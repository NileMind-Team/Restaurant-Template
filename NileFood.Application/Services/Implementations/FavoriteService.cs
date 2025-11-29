using NileFood.Application.Abstractions;
using NileFood.Application.Contracts.Favorites;
using NileFood.Application.Services.Interfaces;
using NileFood.Domain.Entities;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Services.Implementations;
public class FavoriteService(ApplicationDbContext context) : IFavoriteService
{
    private readonly ApplicationDbContext _context = context;


    public async Task<Result<List<FavoriteResponse>>> GetAllForuserAsync(string userId)
    {
        var favorites = await _context.Favorites
            .ProjectToType<FavoriteResponse>()
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(favorites);
    }

    public async Task<Result<FavoriteResponse>> GetAsync(int id)
    {
        var favorite = await _context.Favorites
            .Where(x => x.Id == id)
            .ProjectToType<FavoriteResponse>()
            .FirstOrDefaultAsync();

        return favorite is null ? Result.Failure<FavoriteResponse>(FavoritesErrors.FavoriteNotFound) : Result.Success(favorite);
    }

    public async Task<Result<FavoriteResponse>> CreateAsync(FavoriteRequest request, string userId)
    {
        if (await _context.Favorites.AnyAsync(x => x.MenuItemId == request.MenuItemId && x.UserId == userId))
            return Result.Failure<FavoriteResponse>(FavoritesErrors.FavoriteAlreadyExists);

        if (!await _context.MenuItems.AnyAsync(x => x.Id == request.MenuItemId))
            return Result.Failure<FavoriteResponse>(MenuItemErrors.MenuItemNotFound);

        var favorite = request.Adapt<Favorite>();
        favorite.UserId = userId;

        await _context.AddAsync(favorite);
        await _context.SaveChangesAsync();


        var favoriteResponse = favorite.Adapt<FavoriteResponse>();

        return Result.Success(favoriteResponse);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        if (await _context.Favorites.FirstOrDefaultAsync(x => x.Id == id) is not { } favorite)
            return Result.Failure(FavoritesErrors.FavoriteNotFound);


        _context.Remove(favorite);
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}
