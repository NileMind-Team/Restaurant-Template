namespace NileFood.Domain.Entities;
public class CartItemOption
{
    public int Id { get; set; }

    public int CartItemId { get; set; }
    public CartItem CartItem { get; set; } = null!;

    public int MenuItemOptionId { get; set; }
    public MenuItemOption MenuItemOption { get; set; } = null!;
}
