using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Cities;


namespace NileFood.Application.Services.Interfaces;

public interface ICityService
{
    Task<Result<List<CityResponse>>> GetAllAsync();
    Task<Result<CityResponse>> GetAsync(int id);
    Task<Result<CityResponse>> CreateAsync(CityRequest request);    
    Task<Result> UpdateAsync(int id, CityRequest request);
    Task<Result> DeleteAsync(int id);
}
