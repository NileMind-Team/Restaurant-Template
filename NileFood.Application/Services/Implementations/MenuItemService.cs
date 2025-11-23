using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.MenuItems;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;

public class MenuItemService(ApplicationDbContext context,IFileService fileService) : IMenuItemService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IFileService _fileService = fileService;

    public async Task<Result<List<MenuItemResponse>>> GetAllAsync()
    {
        var menuItems = await _context.MenuItems
            .Include(x=>x.Category)
            .ProjectToType<MenuItemResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(menuItems);
    }

    public async Task<Result<MenuItemResponse>> GetAsync(int id)
    {
        var menuItem = await _context.MenuItems
            .Where(x => x.Id == id)
            .ProjectToType<MenuItemResponse>()
            .FirstOrDefaultAsync();

        return menuItem is null ? Result.Failure<MenuItemResponse>(MenuItemErrors.MenuItemNotFound) : Result.Success(menuItem);
    }


    public async Task<Result<MenuItemResponse>> CreateAsync(MenuItemRequest request)
    {        
        if (await _context.Categories.FindAsync(request.CategoryId) is not { } category)
            return Result.Failure<MenuItemResponse>(CategoryErrors.CategoryNotFound);

        var menuItem = request.Adapt<MenuItem>();

        
        var imageUrl = await _fileService.UploadAsync(request.Image,$"Categories/{category.Name}");
        menuItem.ImageUrl = imageUrl;

        await _context.MenuItems.AddAsync(menuItem);
        await _context.SaveChangesAsync();

        await _context.Entry(menuItem).Reference(r => r.Category).LoadAsync();

        var menuItemResponse = menuItem.Adapt<MenuItemResponse>();

        return Result.Success(menuItemResponse);
    }

    public async Task<Result> UpdateAsync(int id, UpdateMenuItemRequest request)
    {

        if (await _context.MenuItems.FirstOrDefaultAsync(x => x.Id == id) is not { } menuItem)
            return Result.Failure(MenuItemErrors.MenuItemNotFound);

        if (await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId) is not { } category)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        await _fileService.DeleteAsync(menuItem.ImageUrl);
        var newImageUrl = await _fileService.UploadAsync(request.Image, $"Categories/{category.Name}");

        request.Adapt(menuItem);
        menuItem.ImageUrl = newImageUrl;

        _context.Update(menuItem);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
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
    

}
