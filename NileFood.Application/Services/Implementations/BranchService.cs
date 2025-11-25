using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Branches;
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

    public async Task<Result<BranchResponse>> GetAsync(int id)
    {
        var branch = await _context.Branches
            .Where(x => x.Id == id)
            .ProjectToType<BranchResponse>()
            .FirstOrDefaultAsync();

        return branch is null ? Result.Failure<BranchResponse>(BranchErrors.BranchNotFound) : Result.Success(branch);
    }

    public async Task<Result<BranchResponse>> CreateAsync(BranchRequest request)
    {
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


        var cityResponse = branch.Adapt<BranchResponse>();

        return Result.Success(cityResponse);
    }

    public async Task<Result> UpdateAsync(int id, BranchRequest request)
    {
        if (await _context.Branches.FirstOrDefaultAsync(x => x.Id == id) is not { } branch)
            return Result.Failure(BranchErrors.BranchNotFound);

        if (await _context.Branches.AnyAsync(x => x.Name == request.Name && x.Id != id))
            return Result.Failure(BranchErrors.BranchNameAlreadyUsed);

        if (!await _context.Cities.AnyAsync(x => x.Id == request.CityId))
            return Result.Failure<BranchResponse>(CityErrors.CityNotFound);

        if (!await _context.Users.AnyAsync(x => x.Id == request.ManagerId))
            return Result.Failure<BranchResponse>(UserErrors.UserNotFound);

        if (!await _userService.UserHasRoleAsync(request.ManagerId, DefaultRoles.Branch.Name))
            return Result.Failure<BranchResponse>(UserErrors.UserDoesNotHaveBranchRole);

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
}
