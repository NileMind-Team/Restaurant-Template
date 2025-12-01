using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.MenuItemOptions;

namespace NileFood.Application.Services.Interfaces;
public interface IMenuItemOptionService
{
    Task<Result<MenuItemOptionResponse>> CreateAsync(MenuItemOptionDetailsRequest request, string userId);
    Task<Result> UpdateAsync(int id, UpdateMenuItemOptionDetailsRequest request, string userId);
    Task<Result> DeleteAsync(int id, string userId);
}
