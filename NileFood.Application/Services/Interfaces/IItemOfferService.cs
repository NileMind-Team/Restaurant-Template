using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.ItemOffers;

namespace NileFood.Application.Services.Interfaces;
public interface IItemOfferService
{
    Task<Result<List<ItemOfferResponse>>> GetAllActiveAsync();
    Task<Result<ItemOfferResponse>> GetAsync(int id);
    Task<Result<ItemOfferResponse>> CreateAsync(ItemOfferRequest request);
    Task<Result> UpdateAsync(int id, ItemOfferRequest request);
    Task<Result> DeleteAsync(int id);
}
