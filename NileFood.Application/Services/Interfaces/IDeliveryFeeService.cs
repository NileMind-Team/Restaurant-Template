using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.DeliveryFees;

namespace NileFood.Application.Services.Interfaces;
public interface IDeliveryFeeService
{
    Task<Result<List<DeliveryFeeResponse>>> GetAllAsync(int? branchId);
    Task<Result<DeliveryFeeResponse>> GetAsync(int id);
    Task<Result<DeliveryFeeResponse>> CreateAsync(DeliveryFeeRequest request);
    Task<Result> UpdateAsync(int id, DeliveryFeeRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> ChangeActiveStatusAsync(int id);

}
