using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Favorites;

namespace NileFood.Application.Services.Interfaces;
public interface IFavoriteService
{
    Task<Result<List<FavoriteResponse>>> GetAllForuserAsync(string userId);
    Task<Result<FavoriteResponse>> GetAsync(int id);
    Task<Result<FavoriteResponse>> CreateAsync(FavoriteRequest request, string userId);
    Task<Result> DeleteAsync(int id);
}
