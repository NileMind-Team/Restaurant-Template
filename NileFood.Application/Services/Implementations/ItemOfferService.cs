using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.ItemOffers;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class ItemOfferService(ApplicationDbContext context) : IItemOfferService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<ItemOfferResponse>>> GetAllActiveAsync()
    {
        var now = DateTime.UtcNow.AddHours(1);

        var itemOffers = await _context.ItemOffers
            .Where(x => x.IsEnabled &&
                        now >= x.StartDate &&
                        now <= x.EndDate)
            .ProjectToType<ItemOfferResponse>()
            .ToListAsync();

        return Result.Success(itemOffers);
    }

    public async Task<Result<ItemOfferResponse>> GetAsync(int id)
    {
        var itemOffer = await _context.ItemOffers
            .Where(x => x.Id == id)
            .ProjectToType<ItemOfferResponse>()
            .FirstOrDefaultAsync();

        return itemOffer is null ? Result.Failure<ItemOfferResponse>(ItemOfferErrors.ItemOfferNotFound) : Result.Success(itemOffer);
    }

    public async Task<Result<ItemOfferResponse>> CreateAsync(ItemOfferRequest request)
    {

        request.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Local).ToUniversalTime();
        request.EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Local).ToUniversalTime();


        if (!await _context.MenuItems.AnyAsync(x => x.Id == request.MenuItemId))
            return Result.Failure<ItemOfferResponse>(MenuItemErrors.MenuItemNotFound);


        bool isOverlapping = await _context.ItemOffers
            .AnyAsync(x => x.MenuItemId == request.MenuItemId && x.IsEnabled &&
                (
                    (request.StartDate >= x.StartDate && request.StartDate <= x.EndDate) ||
                    (request.EndDate >= x.StartDate && request.EndDate <= x.EndDate) ||
                    (request.StartDate <= x.StartDate && request.EndDate >= x.EndDate)
                )
            );

        if (isOverlapping)
            return Result.Failure<ItemOfferResponse>(ItemOfferErrors.ItemOfferAlreadyExists);

        if (await _context.Branches.CountAsync(x => request.BranchesIds.Contains(x.Id)) != request.BranchesIds.Count)
            return Result.Failure<ItemOfferResponse>(BranchErrors.InvalidBranchIds);

        var itemOffer = request.Adapt<ItemOffer>();
        itemOffer.BranchItemOffers = request.BranchesIds.Select(branchId => new BranchItemOffer { BranchId = branchId }).ToList();




        _context.ItemOffers.Add(itemOffer);
        await _context.SaveChangesAsync();

        var response = itemOffer.Adapt<ItemOfferResponse>();
        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(int id, ItemOfferRequest request)
    {

        request.StartDate = DateTime.SpecifyKind(request.StartDate, DateTimeKind.Local).ToUniversalTime();
        request.EndDate = DateTime.SpecifyKind(request.EndDate, DateTimeKind.Local).ToUniversalTime();

        if (await _context.ItemOffers.Include(x => x.BranchItemOffers).FirstOrDefaultAsync(x => x.Id == id) is not { } itemOffer)
            return Result.Failure<ItemOfferResponse>(ItemOfferErrors.ItemOfferNotFound);


        bool isOverlapping = await _context.ItemOffers
            .AnyAsync(x => x.MenuItemId == request.MenuItemId && x.IsEnabled && x.Id != id &&
                (
                    (request.StartDate >= x.StartDate && request.StartDate <= x.EndDate) ||
                    (request.EndDate >= x.StartDate && request.EndDate <= x.EndDate) ||
                    (request.StartDate <= x.StartDate && request.EndDate >= x.EndDate)
                )
            );

        if (isOverlapping)
            return Result.Failure<ItemOfferResponse>(ItemOfferErrors.ItemOfferAlreadyExists);


        if (await _context.Branches.CountAsync(x => request.BranchesIds.Contains(x.Id)) != request.BranchesIds.Count)
            return Result.Failure<ItemOfferResponse>(BranchErrors.InvalidBranchIds);


        //itemOffer.MenuItemId = request.MenuItemId;
        //itemOffer.StartDate = request.StartDate;
        //itemOffer.EndDate = request.EndDate;
        //itemOffer.IsPercentage = request.IsPercentage;
        //itemOffer.DiscountValue = request.DiscountValue;
        //itemOffer.IsEnabled = request.IsEnabled;

        request.Adapt(itemOffer);


        _context.BranchItemOffers.RemoveRange(itemOffer.BranchItemOffers);

        itemOffer.BranchItemOffers = request.BranchesIds.Select(branchId => new BranchItemOffer { BranchId = branchId, ItemOfferId = itemOffer.Id }).ToList();

        await _context.SaveChangesAsync();


        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.ItemOffers.FindAsync(id) is not { } itemOffer)
            return Result.Failure(ItemOfferErrors.ItemOfferNotFound);

        _context.ItemOffers.Remove(itemOffer);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

}
