using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Orders;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class OrderService(ApplicationDbContext context) : IOrderService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<List<OrderResponse>>> GetAllAsync(string? status, DateOnly? startRange, DateOnly? endRange, string? userId)
    {

        IQueryable<Order> query = _context.Orders;

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
            query = query.Where(o => o.Status == parsedStatus);

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query = query.Where(o => o.UserId == userId);
        }

        if (startRange.HasValue)
        {
            var startDateTime = startRange.Value.ToDateTime(new TimeOnly(0, 0));
            query = query.Where(o => o.CreatedAt >= startDateTime);
        }

        if (endRange.HasValue)
        {
            var endDateTime = endRange.Value.ToDateTime(new TimeOnly(23, 59, 59));
            query = query.Where(o => o.CreatedAt <= endDateTime);
        }


        var orders = await query
            .Include(x => x.DeliveryFee)
            .Include(x => x.Location)
            .OrderByDescending(o => o.CreatedAt).ToListAsync();

        var response = orders.Adapt<List<OrderResponse>>();

        return Result.Success(response);
    }

    public async Task<Result<OrderResponse>> GetAsync(int id)
    {
        var order = await _context.Orders
            .Where(x => x.Id == id)
            .ProjectToType<OrderResponse>()
            .FirstOrDefaultAsync();

        return order is null ? Result.Failure<OrderResponse>(OrderErrors.NotFound) : Result.Success(order);
    }

    public async Task<Result<OrderResponse>> GetForUserAsync(int id, string userId)
    {
        var order = await _context.Orders
               .Where(x => x.Id == id && x.UserId == userId)
               .ProjectToType<OrderResponse>()
               .FirstOrDefaultAsync();

        return order is null ? Result.Failure<OrderResponse>(OrderErrors.NotFound) : Result.Success(order);
    }


    public async Task<Result<OrderResponse>> CreateAsync(OrderRequest request, string userId)
    {
        if (!await _context.Branches.AnyAsync(x => x.Id == request.BranchId))
            return Result.Failure<OrderResponse>(BranchErrors.BranchNotFound);

        if (_context.DeliveryFees.FirstOrDefault(x => x.Id == request.DeliveryFeeId && x.BranchId == request.BranchId) is not { } deliveryFee)
            return Result.Failure<OrderResponse>(DeliveryFeeErrors.DeliveryFeeNotFound);



        var cart = await _context.Carts
            .Include(x => x.CartItems).ThenInclude(ci => ci.MenuItem)
            .Include(x => x.CartItems).ThenInclude(x => x.Options).ThenInclude(o => o.MenuItemOption)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == request.CartId);

        if (cart is null)
            return Result.Failure<OrderResponse>(new Error("Cart", "Cart not found for the user.", StatusCodes.Status404NotFound));

        // Get unavailable cart items directly from the database
        var unavailableCartItems = await _context.CartItems
            .Where(ci => ci.CartId == cart.Id && !_context.BranchMenuItems
            .Where(bm => bm.BranchId == request.BranchId).Select(bm => bm.MenuItemId).Contains(ci.MenuItemId))
            .Select(ci => ci.Id).ToListAsync();

        if (unavailableCartItems.Any())
        {
            return Result.Failure<OrderResponse>(new Error(
                "Cart", $"The following cart item IDs are not available in this branch: {string.Join(", ", unavailableCartItems)}",
                StatusCodes.Status400BadRequest));
        }

        // جمع جميع معرفات الخيارات في عربة التسجيل
        var cartOptionIds = cart.CartItems.SelectMany(ci => ci.Options).Select(x => x.MenuItemOptionId).Distinct().ToList();

        if (cartOptionIds.Any())
        {
            var unavailableOptions = await _context.MenuItemOptions
            .Where(option => cartOptionIds.Contains(option.Id)
                      && !_context.BranchMenuItemOptions
                      .Where(bmo => bmo.BranchMenuItem.BranchId == request.BranchId && bmo.IsActive && bmo.IsAvailable)
                          .Select(bmo => bmo.MenuItemOptionId).Contains(option.Id))
            .Select(option => option.Id).ToListAsync();


            if (unavailableOptions.Any())
            {
                return Result.Failure<OrderResponse>(new Error(
                    "Cart.Options", $"The following option IDs are not available in this branch: {string.Join(", ", unavailableOptions)}",
                    StatusCodes.Status400BadRequest
                ));
            }
        }

        var orderItems = cart.CartItems.Select(ci => new OrderItem
        {
            MenuItemId = ci.MenuItemId,
            Quantity = ci.Quantity,
            MenuItemNameSnapshotAtOrder = ci.MenuItem.Name,
            MenuItemDescriptionAtOrder = ci.MenuItem.Description,
            MenuItemBasePriceSnapshotAtOrder = ci.MenuItem.BasePrice,
            MenuItemImageUrlSnapshotAtOrder = ci.MenuItem.ImageUrl,
            Options = ci.Options.Select(o => new OrderItemOption
            {
                MenuItemOptionId = o.MenuItemOptionId,
                OptionNameAtOrder = o.MenuItemOption.Name,
                OptionPriceAtOrder = o.MenuItemOption.Price
            }).ToList()
        }).ToList();

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user!.DefaultLocationId is null)
                return Result.Failure<OrderResponse>(new Error("User.DefaultLocation", "User must have a default location.", StatusCodes.Status400BadRequest));


            var orderNumber = await GenerateOrderNumberAsync();
            var order = new Order()
            {
                UserId = userId,
                BranchId = request.BranchId,
                DeliveryFeeId = request.DeliveryFeeId,
                LocationId = user.DefaultLocationId.Value,
                OrderNumber = orderNumber,
                Notes = request.Notes,
                Items = orderItems
            };

            order.RecalculateTotals(deliveryFee.Fee, request.Discount);


            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // هنا يجب مسح عربة التسجيل بعد إنشاء الطلب
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();


            await _context.Entry(order).Reference(o => o.DeliveryFee).LoadAsync();
            await _context.Entry(order).Reference(o => o.Location).LoadAsync();


            var orderResponse = order.Adapt<OrderResponse>();

            return Result.Success(orderResponse);

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return Result.Failure<OrderResponse>(new Error("Order", $"Error creating order: {ex.Message}", StatusCodes.Status500InternalServerError));
        }




    }

    public Task<Result> UpdateAsync(int id, OrderRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    private async Task<string> GenerateOrderNumberAsync()
    {
        var lastOrder = await _context.Orders
            .OrderByDescending(o => o.Id)
            .FirstOrDefaultAsync();

        var nextNumber = lastOrder?.Id + 1 ?? 1;

        return $"ORD-{nextNumber}";
    }


}


