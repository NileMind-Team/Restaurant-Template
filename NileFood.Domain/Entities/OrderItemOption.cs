namespace NileFood.Domain.Entities;
public class OrderItemOption
{
    public int Id { get; set; }

    public int OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = null!;

    public int MenuItemOptionId { get; set; }
    public MenuItemOption MenuItemOption { get; set; } = null!;

    public string OptionNameAtOrder { get; set; } = null!;
    public decimal OptionPriceAtOrder { get; set; }
}

