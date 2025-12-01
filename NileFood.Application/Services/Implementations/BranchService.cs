using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Branches;
using NileFood.Application.Contracts.PhoneNumbers;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class BranchService(ApplicationDbContext context, IUserService userService) : IBranchService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IUserService _userService = userService;

    public async Task<Result<List<BranchResponse>>> GetAllAsync()
    {
        var branches = await _context.Branches
            .ProjectToType<BranchResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(branches);
    }

    public async Task<Result<List<ListBranchResponse>>> GetListAsync()
    {
        var branches = await _context.Branches
            .Include(x => x.City)
            .Include(x => x.PhoneNumbers)
            .ProjectToType<ListBranchResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(branches);
    }

    public async Task<Result<BranchResponse>> GetAsync(int id)
    {
        var branch = await _context.Branches
            .Include(x => x.City)
            .Include(x => x.PhoneNumbers)
            .Where(x => x.Id == id)
            .ProjectToType<BranchResponse>()
            .FirstOrDefaultAsync();

        return branch is null ? Result.Failure<BranchResponse>(BranchErrors.BranchNotFound) : Result.Success(branch);
    }

    public async Task<Result<BranchResponse>> CreateAsync(BranchRequest request)
    {
        if (request.PhoneNumbers.Any(x => x.Id.HasValue))
            return Result.Failure<BranchResponse>(BranchErrors.InvalidIds);

        if (!await _context.Cities.AnyAsync(x => x.Id == request.CityId))
            return Result.Failure<BranchResponse>(CityErrors.CityNotFound);

        if (await _context.Branches.AnyAsync(x => x.Name == request.Name))
            return Result.Failure<BranchResponse>(BranchErrors.BranchNameAlreadyUsed);

        if (!await _context.Users.AnyAsync(x => x.Id == request.ManagerId))
            return Result.Failure<BranchResponse>(UserErrors.UserNotFound);

        if (!await _userService.UserHasRoleAsync(request.ManagerId, DefaultRoles.Branch.Name))
            return Result.Failure<BranchResponse>(UserErrors.UserDoesNotHaveBranchRole);


        var branch = request.Adapt<Branch>();

        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();

        await _context.Entry(branch).Reference(r => r.City).LoadAsync();


        var branchResponse = branch.Adapt<BranchResponse>();

        return Result.Success(branchResponse);
    }

    public async Task<Result> UpdateAsync(int id, BranchRequest request)
    {
        if (await _context.Branches.Include(x => x.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == id) is not { } branch)
            return Result.Failure(BranchErrors.BranchNotFound);

        if (await _context.Branches.AnyAsync(x => x.Name == request.Name && x.Id != id))
            return Result.Failure(BranchErrors.BranchNameAlreadyUsed);

        if (!await _context.Cities.AnyAsync(x => x.Id == request.CityId))
            return Result.Failure<BranchResponse>(CityErrors.CityNotFound);

        if (!await _context.Users.AnyAsync(x => x.Id == request.ManagerId))
            return Result.Failure<BranchResponse>(UserErrors.UserNotFound);

        if (!await _userService.UserHasRoleAsync(request.ManagerId, DefaultRoles.Branch.Name))
            return Result.Failure<BranchResponse>(UserErrors.UserDoesNotHaveBranchRole);


        var scheduleResult = UpdatePhoneNumbers(branch, request.PhoneNumbers);
        if (scheduleResult.IsFailure)
            return scheduleResult;

        request.Adapt(branch);

        _context.Update(branch);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ChangeActiveStatusAsync(int id)
    {
        if (id <= 0)
            return Result.Failure(BranchErrors.InvalidBranchId);

        if (await _context.Branches.FirstOrDefaultAsync(x => x.Id == id) is not { } branch)
            return Result.Failure(MenuItemErrors.MenuItemNotFound);

        branch.IsActive = !branch.IsActive;
        await _context.SaveChangesAsync();

        return Result.Success();
    }




    private Result UpdatePhoneNumbers(Branch branch, List<PhoneNumberRequest> incomingPhoneNumbers)
    {
        var existingPhoneNumber = branch.PhoneNumbers.ToList();
        var existingIds = existingPhoneNumber.Select(s => s.Id).ToHashSet();

        var incomingIds = incomingPhoneNumbers.Where(s => s.Id.HasValue).Select(s => s.Id!.Value).ToList();


        var invalidIds = incomingIds.Where(id => !existingIds.Contains(id)).ToList();
        if (invalidIds.Any())
            return Result.Failure(MenuItemErrors.InvalidIds);


        var phoneNumberToRemove = existingPhoneNumber.Where(s => !incomingIds.Contains(s.Id)).ToList();

        foreach (var removed in phoneNumberToRemove)
            branch.PhoneNumbers.Remove(removed);


        foreach (var phoneNumberRequest in incomingPhoneNumbers)
        {
            if (phoneNumberRequest.Id.HasValue)
            {
                var phoneNumber = existingPhoneNumber.First(s => s.Id == phoneNumberRequest.Id);
                phoneNumberRequest.Adapt(phoneNumber);
            }
            else
            {
                var newPhoneNumber = phoneNumberRequest.Adapt<PhoneNumber>();
                newPhoneNumber.BranchId = branch.Id;
                branch.PhoneNumbers.Add(newPhoneNumber);
            }
        }

        return Result.Success();
    }
}
