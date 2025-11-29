using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.DeliveryFees;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class DeliveryFeeService(ApplicationDbContext context) : IDeliveryFeeService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<List<DeliveryFeeResponse>>> GetAllAsync(int? branchId)
    {
        var deliveryFees = await _context.DeliveryFees
            .Where(x => !branchId.HasValue || x.BranchId == branchId.Value)
            .ProjectToType<DeliveryFeeResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(deliveryFees);
    }

    public async Task<Result<DeliveryFeeResponse>> GetAsync(int id)
    {
        var deliveryFee = await _context.DeliveryFees
            .Where(x => x.Id == id)
            .ProjectToType<DeliveryFeeResponse>()
            .FirstOrDefaultAsync();

        return deliveryFee is null ? Result.Failure<DeliveryFeeResponse>(DeliveryFeeErrors.DeliveryFeeNotFound) : Result.Success(deliveryFee);
    }

    public async Task<Result<DeliveryFeeResponse>> CreateAsync(DeliveryFeeRequest request)
    {
        if (!await _context.Branches.AnyAsync(x => x.Id == request.BranchId))
            return Result.Failure<DeliveryFeeResponse>(BranchErrors.BranchNotFound);

        if (await _context.DeliveryFees.AnyAsync(x => x.AreaName == request.AreaName && x.BranchId == request.BranchId))
            return Result.Failure<DeliveryFeeResponse>(DeliveryFeeErrors.DeliveryFeeAlreadyExists);

        var deliveryFee = request.Adapt<DeliveryFee>();

        await _context.AddAsync(deliveryFee);
        await _context.SaveChangesAsync();


        var deliveryFeeResponse = deliveryFee.Adapt<DeliveryFeeResponse>();

        return Result.Success(deliveryFeeResponse);
    }

    public async Task<Result> UpdateAsync(int id, DeliveryFeeRequest request)
    {
        if (await _context.DeliveryFees.FirstOrDefaultAsync(x => x.Id == id) is not { } deliveryFee)
            return Result.Failure(DeliveryFeeErrors.DeliveryFeeNotFound);

        if (await _context.DeliveryFees.AnyAsync(x => x.Id != id && x.BranchId == request.BranchId && x.AreaName == request.AreaName))
            return Result.Failure<DeliveryFeeResponse>(DeliveryFeeErrors.DeliveryFeeAlreadyExists);

        request.Adapt(deliveryFee);

        _context.Update(deliveryFee);
        await _context.SaveChangesAsync();

        return Result.Success();
    }



    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.DeliveryFees.FirstOrDefaultAsync(x => x.Id == id) is not { } deliveryFee)
            return Result.Failure(DeliveryFeeErrors.DeliveryFeeNotFound);


        _context.Remove(deliveryFee);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

}
