using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.MenuItemOptionTypes;

namespace NileFood.Application.Services.Interfaces;
public interface IMenuItemOptionTypeService
{
    Task<Result<List<MenuItemOptionTypeResponse>>> GetAllAsync();
    Task<Result<MenuItemOptionTypeResponse>> GetAsync(int id);
    Task<Result<MenuItemOptionTypeResponse>> CreateAsync(MenuItemOptionTypeRequest request);
    Task<Result> UpdateAsync(int id, MenuItemOptionTypeRequest request);
    Task<Result> DeleteAsync(int id);
}
