using Egyptos.Application.Abstractions;
using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Common;
using NileFood.Application.Contracts.MenuItems;


namespace NileFood.Application.Services.Interfaces;

public interface IMenuItemService
{
    Task<Result<PaginatedList<MenuItemResponse>>> GetAllAsync(List<FilterDto> filters, UserParams userParams, int? categoryId);
    Task<Result<List<MenuItemResponse>>> GetAllAsync(int? categoryId);
    Task<Result<MenuItemResponse>> GetAsync(int id);
    Task<Result<MenuItemResponse>> CreateAsync(MenuItemRequest request, string userId);
    Task<Result> UpdateAsync(int id, UpdateMenuItemRequest request);
    Task<Result> DeleteAsync(int id);

    Task<Result> ChangeMenuItemActiveStatusAsync(int id);
}
