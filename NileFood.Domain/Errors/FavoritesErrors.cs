using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class FavoritesErrors
{
    public static readonly Error FavoriteNotFound = new("Favorite.NotFound", "Favorite is not found ", StatusCodes.Status404NotFound);
    public static readonly Error FavoriteNameAlreadyUsed = new("Favorite.NameAlreadyUsed", "Favorite name is already used.", StatusCodes.Status409Conflict);
    public static readonly Error FavoriteAlreadyExists = new("Favorite.AlreadyExists", "This item has already been added to favorites.", StatusCodes.Status409Conflict);
}
