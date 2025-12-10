namespace NileFood.Domain.Entities;
public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public int Quantity { get; set; }

    public List<OrderItemOption> Options { get; set; } = [];


    // --- Snapshot fields ---
    public string MenuItemNameSnapshotAtOrder { get; set; } = null!;
    public string? MenuItemDescriptionAtOrder { get; set; }
    public decimal MenuItemBasePriceSnapshotAtOrder { get; set; }
    public string MenuItemImageUrlSnapshotAtOrder { get; set; } = null!;


    // --- Total price (based on snapshot) ---
    public decimal TotalPrice =>
        (MenuItemBasePriceSnapshotAtOrder * Quantity) +
        Options.Sum(x => x.OptionPriceAtOrder);
}
