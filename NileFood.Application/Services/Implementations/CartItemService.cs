using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.CartItems;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class CartItemService(ApplicationDbContext context) : ICartItemService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<List<CartItemResponse>>> GetAllForUserAsync(string userId)
    {
        var cartItems = await _context.CartItems
            .Include(x => x.Options)
            .ThenInclude(x => x.MenuItemOption)
            .Include(x => x.MenuItem)
            .Include(x => x.Cart)
            .Where(ci => ci.Cart.UserId == userId)
            .ProjectToType<CartItemResponse>()
            .ToListAsync();


        return Result.Success(cartItems);
    }


    //public async Task<Result<CartItemResponse>> GetAsync(int id)
    //{
    //    var cartItem = await _context.CartItems
    //        .Where(x => x.Id == id)
    //        .Include(x => x.Options)
    //        .ThenInclude(x => x.MenuItemOption)
    //        .Include(x => x.MenuItem)
    //        .Include(x => x.Cart)
    //        .ProjectToType<CartItemResponse>()
    //        .FirstOrDefaultAsync();

    //    return cartItem is null ? Result.Failure<CartItemResponse>(CartItemErrors.CityNotFound) : Result.Success(city);
    //}


    public async Task<Result> CreateAsync(CartItemRequest request, string userId)
    {

        if (!await _context.MenuItems.AnyAsync(x => x.Id == request.MenuItemId))
            return Result.Failure<CartItemResponse>(MenuItemErrors.MenuItemNotFound);

        var validOptionIds = await _context.BranchMenuItemOptions
            .Where(o => o.BranchMenuItem.MenuItemId == request.MenuItemId && o.IsActive && o.IsAvailable)
            .Select(o => o.MenuItemOptionId)
            .ToListAsync();

        var invalidOptions = request.Options.Except(validOptionIds).ToList();
        if (invalidOptions.Any())
        {
            var errorMessages = invalidOptions
                .Select(id => $"Option {id} is not valid for the selected menu item.")
                .ToList();

            return Result.Failure<CartItemResponse>(new Error("InvalidOptions", string.Join(", ", errorMessages), 400));
        }


        var cart = _context.Carts.Include(c => c.CartItems).ThenInclude(x => x.Options).FirstOrDefault(c => c.UserId == userId);
        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }


        var existingCartItem = cart.CartItems
            .FirstOrDefault(ci => ci.MenuItemId == request.MenuItemId && ci.Options.Select(o => o.MenuItemOptionId)
            .OrderBy(x => x).SequenceEqual(request.Options.OrderBy(x => x)));

        if (existingCartItem != null)
            existingCartItem.Quantity += request.Quantity;
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                MenuItemId = request.MenuItemId,
                Quantity = request.Quantity,
                Options = request.Options.Select(optId => new CartItemOption { MenuItemOptionId = optId }).ToList()
            };

            cart.CartItems.Add(cartItem);
        }

        await _context.SaveChangesAsync();


        return Result.Success();
    }


    public Task<Result> UpdateAsync(int id, CartItemRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UpdateQuantityAsync(int id, CartItemQuantityRequest request)
    {
        // Check existence first
        var exists = await _context.CartItems.AnyAsync(c => c.Id == id);
        if (!exists)
            return Result.Failure(CartItemErrors.CartItemNotFound);

        // Update directly
        await _context.CartItems
            .Where(c => c.Id == request.MenuItemId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(c => c.Quantity, request.Quantity)
            );

        return Result.Success();
    }


    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.CartItems.FirstOrDefaultAsync(x => x.Id == id) is not { } cartItem)
            return Result.Failure(CartItemErrors.CartItemNotFound);

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}
