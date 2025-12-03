using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.CartItems;

namespace NileFood.Application.Services.Interfaces;
public interface ICartItemService
{
    Task<Result<List<CartItemResponse>>> GetAllForUserAsync(string userId);
    //Task<Result<CartItemResponse>> GetAsync(int id);
    Task<Result> CreateAsync(CartItemRequest request, string userId);
    Task<Result> UpdateAsync(int id, CartItemRequest request);
    Task<Result> DeleteAsync(int id);
}
