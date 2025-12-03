using Egyptos.Application.Abstractions;
using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.BranchMenuItemOptions;
using NileFood.Application.Contracts.Categories;
using NileFood.Application.Contracts.Common;
using NileFood.Application.Contracts.ItemOffers;
using NileFood.Application.Contracts.MenuItemOptions;
using NileFood.Application.Contracts.MenuItems;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Consts;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;

public class MenuItemService(ApplicationDbContext context, IUserService userService, IFileService fileService, IFilterService<MenuItem> filterService) : IMenuItemService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IUserService _userService = userService;
    private readonly IFileService _fileService = fileService;
    private readonly IFilterService<MenuItem> _filterService = filterService;


    public async Task<Result<PaginatedList<MenuItemResponse>>> GetAllAsync(List<FilterDto> filters, UserParams userParams, int? categoryId)
    {

        var filtered = await _filterService.Filter(filters, userParams, null, null);


        var x = new PaginatedList<MenuItemResponse>(null, 1, 1, 1);

        return Result.Success(x);
    }


    public async Task<Result<List<MenuItemResponse>>> GetAllAsync(int? categoryId)
    {
        var menuItems = await _context.MenuItems
            .Where(x => !categoryId.HasValue || x.CategoryId == categoryId)
            .Include(x => x.Category)
            .Include(x => x.ItemOffers)
            .AsNoTracking()
            .ProjectToType<MenuItemResponse>()
            .ToListAsync();



        return Result.Success(menuItems);
    }

    public async Task<Result<MenuItemDetailsResponse>> GetAsync(int id)
    {
        var menuItemResponse = await _context.MenuItems
            .Where(x => x.Id == id)
            .Select(x => new MenuItemDetailsResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                BasePrice = x.BasePrice,
                ImageUrl = x.ImageUrl,
                IsAllTime = x.IsAllTime,
                IsActive = x.IsActive,
                Calories = x.Calories,
                PreparationTimeStart = x.PreparationTimeStart,
                PreparationTimeEnd = x.PreparationTimeEnd,

                Category = x.Category.Adapt<CategoryResponse>(),
                ItemOffer = x.ItemOffers.FirstOrDefault(o => o.IsEnabled && DateTime.UtcNow.AddHours(1) >= o.StartDate && DateTime.UtcNow.AddHours(1) <= o.EndDate)
                .Adapt<ItemOfferResponse>(),

                MenuItemSchedules = x.MenuItemSchedules.Adapt<List<MenuItemScheduleResponse>>(),

                BranchMenuItems = x.BranchMenuItems.Adapt<List<BranchMenuItemResponse>>(),


                TypesWithOptions = x.BranchMenuItems
                    .SelectMany(b => b.BranchMenuItemOptions)
                    .GroupBy(o => o.MenuItemOption.TypeId)
                    .Select(typeGroup => new OptionTypeWithOptions
                    {
                        Id = typeGroup.First().MenuItemOption.Type.Id,
                        Name = typeGroup.First().MenuItemOption.Type.Name,
                        CanSelectMultipleOptions = typeGroup.First().MenuItemOption.Type.CanSelectMultipleOptions,
                        IsSelectionRequired = typeGroup.First().MenuItemOption.Type.IsSelectionRequired,

                        MenuItemOptions = typeGroup.GroupBy(o => o.MenuItemOptionId)
                            .Select(optGroup => new MenuItemOptionResponse
                            {
                                Id = optGroup.First().MenuItemOptionId,
                                Name = optGroup.First().MenuItemOption.Name,
                                Price = optGroup.First().MenuItemOption.Price,
                                BranchMenuItemOption = optGroup
                                    .Select(x => new BranchMenuItemOptionResponse
                                    {
                                        Id = x.Id,
                                        BranchId = x.BranchMenuItem.BranchId,
                                        IsActive = x.IsActive,
                                        IsAvailable = x.IsAvailable
                                    })
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();


        return menuItemResponse is null ? Result.Failure<MenuItemDetailsResponse>(MenuItemErrors.MenuItemNotFound) : Result.Success(menuItemResponse);
    }



    public async Task<Result<MenuItemResponse>> CreateAsync(MenuItemRequest request, string userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (await _context.Categories.FindAsync(request.CategoryId) is not { } category)
                return Result.Failure<MenuItemResponse>(CategoryErrors.CategoryNotFound);

            var menuItem = request.Adapt<MenuItem>();
            menuItem.ImageUrl = await _fileService.UploadAsync(request.Image, $"Categories/{category.Name}");
            menuItem.IsAllTime = !menuItem.MenuItemSchedules.Any();

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            // 1) options once
            var options = request.MenuItemOptions.Adapt<List<MenuItemOption>>();
            _context.MenuItemOptions.AddRange(options);
            await _context.SaveChangesAsync();

            var roles = await _userService.GetUserRolesAsync(userId);
            bool isAdmin = roles.Contains(DefaultRoles.Admin.Name) || roles.Contains(DefaultRoles.Restaurant.Name);

            List<Branch> branches;

            if (isAdmin)
                branches = await _context.Branches.ToListAsync();
            else
            {
                branches = await _context.Branches
                    .Where(x => x.ManagerId == userId)
                    .ToListAsync();
            }

            if (!branches.Any() && (request.MenuItemOptions.Any() || request.MenuItemSchedules.Any()))
                return Result.Failure<MenuItemResponse>(BranchErrors.NoBranchesForManager);

            var branchMenuItems = branches.Select(branch => new BranchMenuItem
            {
                BranchId = branch.Id,
                MenuItemId = menuItem.Id,
                IsAvailable = true
            }).ToList();

            _context.BranchMenuItems.AddRange(branchMenuItems);
            await _context.SaveChangesAsync();

            // link options to branches
            var branchMenuItemOptions = branchMenuItems
                .SelectMany(bmi => options.Select(opt => new BranchMenuItemOption
                {
                    BranchMenuItemId = bmi.Id,
                    MenuItemOptionId = opt.Id,
                    IsActive = true,
                    IsAvailable = true
                }))
                .ToList();

            _context.BranchMenuItemOptions.AddRange(branchMenuItemOptions);
            await _context.SaveChangesAsync();


            await transaction.CommitAsync();

            await _context.Entry(menuItem).Reference(r => r.Category).LoadAsync();

            return Result.Success(menuItem.Adapt<MenuItemResponse>());
        }
        catch (Exception ex)
        {
            // roll back 
            await transaction.RollbackAsync();

            return Result.Failure<MenuItemResponse>(new("TransactionFailed", $"An error occurred: {ex.Message}", StatusCodes.Status400BadRequest));
        }
    }


    public async Task<Result> UpdateAsync(int id, UpdateMenuItemRequest request)
    {

        if (await _context.MenuItems.Include(x => x.MenuItemSchedules).FirstOrDefaultAsync(x => x.Id == id) is not { } menuItem)
            return Result.Failure(MenuItemErrors.MenuItemNotFound);

        if (await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId) is not { } category)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        // Handle image update
        await _fileService.DeleteAsync(menuItem.ImageUrl);
        var imageUrl = await _fileService.UploadAsync(request.Image, $"Categories/{category.Name}");

        // Update schedules
        var scheduleResult = UpdateMenuItemSchedules(menuItem, request.MenuItemSchedules);
        if (scheduleResult.IsFailure)
            return scheduleResult;


        request.Adapt(menuItem);

        if (!menuItem.MenuItemSchedules.Any()) menuItem.IsAllTime = false;

        menuItem.IsAllTime = !menuItem.MenuItemSchedules.Any();
        menuItem.ImageUrl = imageUrl;

        _context.Update(menuItem);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.MenuItems.FirstOrDefaultAsync(x => x.Id == id) is not { } menuItem)
            return Result.Failure(MenuItemErrors.MenuItemNotFound);

        _context.MenuItems.Remove(menuItem);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> ChangeMenuItemActiveStatusAsync(int id)
    {
        if (id <= 0)
            return Result.Failure(MenuItemErrors.InvalidMenuItemId);

        if (await _context.MenuItems.FirstOrDefaultAsync(x => x.Id == id) is not { } menuItem)
            return Result.Failure(MenuItemErrors.MenuItemNotFound);

        menuItem.IsActive = !menuItem.IsActive;
        await _context.SaveChangesAsync();

        return Result.Success();
    }



    private Result UpdateMenuItemSchedules(MenuItem menuItem, List<MenuItemScheduleRequest> incomingSchedules)
    {
        var existingSchedules = menuItem.MenuItemSchedules.ToList();
        var existingIds = existingSchedules.Select(s => s.Id).ToHashSet();

        var incomingIds = incomingSchedules.Where(s => s.Id.HasValue).Select(s => s.Id!.Value).ToList();

        // 1) validate IDs
        var invalidIds = incomingIds.Where(id => !existingIds.Contains(id)).ToList();
        if (invalidIds.Any())
            return Result.Failure(MenuItemErrors.InvalidIds);

        // 2) remove schedules not sent
        var schedulesToRemove = existingSchedules.Where(s => !incomingIds.Contains(s.Id)).ToList();

        foreach (var removed in schedulesToRemove)
            menuItem.MenuItemSchedules.Remove(removed);

        // 3) update + add
        foreach (var scheduleRequest in incomingSchedules)
        {
            if (scheduleRequest.Id.HasValue)
            {
                var schedule = existingSchedules.First(s => s.Id == scheduleRequest.Id);
                scheduleRequest.Adapt(schedule);
            }
            else
            {
                var newSchedule = scheduleRequest.Adapt<MenuItemSchedule>();
                newSchedule.MenuItemId = menuItem.Id;
                menuItem.MenuItemSchedules.Add(newSchedule);
            }
        }

        return Result.Success();
    }

}
