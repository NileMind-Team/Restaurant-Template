namespace NileFood.Application.Contracts.Favorites;
public class FavoriteResponse
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }

    public string UserId { get; set; } = null!;
}
