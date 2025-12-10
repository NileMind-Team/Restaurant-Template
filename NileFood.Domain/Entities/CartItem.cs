namespace NileFood.Domain.Entities;
public class CartItem
{
    public int Id { get; set; }

    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;


    public int Quantity { get; set; }
    public bool IsOfferApplied { get; set; }


    public List<CartItemOption> Options { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public decimal TotalPrice => (MenuItem.BasePrice * Quantity) + (Quantity * Options.Sum(x => x.MenuItemOption.Price));
}
