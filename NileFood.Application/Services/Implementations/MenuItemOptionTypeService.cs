using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.MenuItemOptionTypes;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class MenuItemOptionTypeService(ApplicationDbContext context) : IMenuItemOptionTypeService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<List<MenuItemOptionTypeResponse>>> GetAllAsync()
    {
        var menuItemOptionTypes = await _context.MenuItemOptionTypes
            .ProjectToType<MenuItemOptionTypeResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(menuItemOptionTypes);
    }

    public async Task<Result<MenuItemOptionTypeResponse>> GetAsync(int id)
    {
        var menuItemOptionType = await _context.MenuItemOptionTypes
            .Where(x => x.Id == id)
            .ProjectToType<MenuItemOptionTypeResponse>()
            .FirstOrDefaultAsync();

        return menuItemOptionType is null ? Result.Failure<MenuItemOptionTypeResponse>(MenuItemOptionTypeErrors.MenuItemOptionTypeNotFound) : Result.Success(menuItemOptionType);
    }

    public async Task<Result<MenuItemOptionTypeResponse>> CreateAsync(MenuItemOptionTypeRequest request)
    {
        if (await _context.MenuItemOptionTypes.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower()))
            return Result.Failure<MenuItemOptionTypeResponse>(MenuItemOptionTypeErrors.MenuItemOptionTypeNameAlreadyUsed);

        var menuItemOptionType = request.Adapt<MenuItemOptionType>();

        await _context.MenuItemOptionTypes.AddAsync(menuItemOptionType);
        await _context.SaveChangesAsync();


        var menuItemOptionTypeResponse = menuItemOptionType.Adapt<MenuItemOptionTypeResponse>();

        return Result.Success(menuItemOptionTypeResponse);
    }


    public async Task<Result> UpdateAsync(int id, MenuItemOptionTypeRequest request)
    {
        if (await _context.MenuItemOptionTypes.FirstOrDefaultAsync(x => x.Id == id) is not { } menuItemOptionType)
            return Result.Failure(MenuItemOptionTypeErrors.MenuItemOptionTypeNotFound);

        if (await _context.MenuItemOptionTypes.AnyAsync(x => x.Name == request.Name && x.Id != id))
            return Result.Failure(MenuItemOptionTypeErrors.MenuItemOptionTypeNameAlreadyUsed);

        request.Adapt(menuItemOptionType);

        _context.Update(menuItemOptionType);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.MenuItemOptionTypes.FirstOrDefaultAsync(x => x.Id == id) is not { } menuItemOptionType)
            return Result.Failure(CityErrors.CityNotFound);


        _context.Remove(menuItemOptionType);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
