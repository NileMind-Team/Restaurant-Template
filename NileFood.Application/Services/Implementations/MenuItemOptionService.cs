using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.MenuItemOptions;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;
using System.Data;


namespace NileFood.Application.Services.Implementations;
public class MenuItemOptionService(ApplicationDbContext context, IUserService userService) : IMenuItemOptionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IUserService _userService = userService;


    public async Task<Result<MenuItemOptionResponse>> CreateAsync(MenuItemOptionDetailsRequest request, string userId)
    {
        if (!await _context.MenuItems.AnyAsync(x => x.Id == request.MenuItemId))
            return Result.Failure<MenuItemOptionResponse>(MenuItemErrors.MenuItemNotFound);

        if (!await _context.MenuItemOptionTypes.AnyAsync(x => x.Id == request.TypeId))
            return Result.Failure<MenuItemOptionResponse>(MenuItemOptionTypeErrors.MenuItemOptionTypeNotFound);

        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var option = new MenuItemOption { TypeId = request.TypeId, Name = request.Name, Price = request.Price };
            _context.MenuItemOptions.Add(option);
            await _context.SaveChangesAsync();

            var roles = await _userService.GetUserRolesAsync(userId);
            bool isAdmin = roles.Contains(DefaultRoles.Admin.Name) || roles.Contains(DefaultRoles.Restaurant.Name);


            var branches = await _context.Branches
                .Where(x => isAdmin || x.ManagerId == userId)
                .ToListAsync();

            if (branches.Count == 0)
                return Result.Failure<MenuItemOptionResponse>(BranchErrors.NoBranchesForManager);

            var branchMenuItems = _context.BranchMenuItems
                .Where(bmi => branches.Select(b => b.Id).Contains(bmi.BranchId) && bmi.MenuItemId == request.MenuItemId)
                .ToList();


            bool exists = await _context.BranchMenuItemOptions
                .AnyAsync(o => o.BranchMenuItem.MenuItemId == request.MenuItemId && o.MenuItemOption.Name == request.Name
                        && o.MenuItemOption.TypeId == request.TypeId);
            if (exists)
                return Result.Failure<MenuItemOptionResponse>(MenuItemOptionErrors.MenuItemOptionAlreadyExists);


            // link options to branches
            var branchMenuItemOptions = branchMenuItems
                .Select(bmi => new BranchMenuItemOption
                {
                    BranchMenuItemId = bmi.Id,
                    MenuItemOptionId = option.Id,
                    IsActive = true,
                    IsAvailable = true
                })
                .ToList();
            _context.BranchMenuItemOptions.AddRange(branchMenuItemOptions);
            await _context.SaveChangesAsync();



            await transaction.CommitAsync();

            return Result.Success(option.Adapt<MenuItemOptionResponse>());
        }
        catch (Exception ex)
        {
            // roll back 
            await transaction.RollbackAsync();

            return Result.Failure<MenuItemOptionResponse>(new("TransactionFailed", $"An error occurred: {ex.Message}", StatusCodes.Status400BadRequest));
        }

    }

    public async Task<Result> DeleteAsync(int id, string userId)
    {

        if (await _context.MenuItemOptions.FirstOrDefaultAsync(x => x.Id == id) is not { } option)
            return Result.Failure(MenuItemOptionErrors.MenuItemOptionNotFound);


        var roles = await _userService.GetUserRolesAsync(userId);
        bool isAdmin = roles.Contains(DefaultRoles.Admin.Name) || roles.Contains(DefaultRoles.Restaurant.Name);


        if (isAdmin)
        {
            _context.MenuItemOptions.Remove(option);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        var branches = await _context.Branches.Where(x => x.ManagerId == userId).Select(x => x.Id).ToListAsync();

        if (branches.Count == 0)
            return Result.Failure(BranchErrors.NoBranchesForManager);

        var branchMenuItemOptions = await _context.BranchMenuItemOptions
            .Where(x => x.MenuItemOptionId == id && branches.Contains(x.BranchMenuItem.BranchId))
            .ToListAsync();

        if (branchMenuItemOptions.Count == 0)
            return Result.Failure(MenuItemOptionErrors.MenuItemOptionNotFound);

        _context.BranchMenuItemOptions.RemoveRange(branchMenuItemOptions);

        await _context.SaveChangesAsync();

        return Result.Success();
    }


    public async Task<Result> UpdateAsync(int id, UpdateMenuItemOptionDetailsRequest request, string userId)
    {
        var option = await _context.MenuItemOptions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (option is null)
            return Result.Failure(MenuItemOptionErrors.MenuItemOptionNotFound);

        bool typeExists = await _context.MenuItemOptionTypes
            .AnyAsync(x => x.Id == request.TypeId);

        if (!typeExists)
            return Result.Failure(MenuItemOptionTypeErrors.MenuItemOptionTypeNotFound);

        var roles = await _userService.GetUserRolesAsync(userId);
        bool isAdmin = roles.Contains(DefaultRoles.Admin.Name)
                    || roles.Contains(DefaultRoles.Restaurant.Name);

        if (!isAdmin)
        {
            var branchIds = await _context.BranchMenuItemOptions
                .Where(x => x.MenuItemOptionId == id)
                .Select(x => x.BranchMenuItem.BranchId)
                .Distinct()
                .ToListAsync();

            if (branchIds.Count > 1)
                return Result.Failure(MenuItemOptionErrors.CannotBeUpdatedDueToMultipleBranches);

            if (branchIds.Count == 1)
            {
                bool isManager = await _context.Branches
                    .AnyAsync(b => b.Id == branchIds[0] && b.ManagerId == userId);

                if (!isManager)
                    return Result.Failure(MenuItemOptionErrors.NotManagerOfThisBranch);
            }
        }

        request.Adapt(option);

        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
