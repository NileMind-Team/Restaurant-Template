using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Locations;

namespace NileFood.Application.Services.Interfaces;

public interface ILocationService
{
    Task<Result<List<LocationResponse>>> GetAllForUserAsync(string usreId);
    Task<Result<LocationResponse>> GetAsync(int id);
    Task<Result<LocationResponse>> CreateAsync(string userId ,LocationRequest request);
    Task<Result> ChangeDefaultLocationAsync(int id,string userId);
    Task<Result> UpdateAsync(int id, LocationRequest request);
    Task<Result> DeleteAsync(int id);
}
