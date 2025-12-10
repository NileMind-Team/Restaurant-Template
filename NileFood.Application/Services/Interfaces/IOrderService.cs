using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Orders;

namespace NileFood.Application.Services.Interfaces;
public interface IOrderService
{
    Task<Result<List<OrderResponse>>> GetAllAsync(string? status, DateOnly? startRange, DateOnly? endRange, string? userId);
    Task<Result<OrderResponse>> GetAsync(int id);
    Task<Result<OrderResponse>> GetForUserAsync(int id, string userId);
    Task<Result<OrderResponse>> CreateAsync(OrderRequest request, string userId);
    Task<Result> UpdateAsync(int id, OrderRequest request);
    Task<Result> DeleteAsync(int id);
}
