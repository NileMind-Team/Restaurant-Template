using NileFood.Application.Contracts.MenuItems;

namespace NileFood.Application.Contracts.Orders;
public class OrderItemResponse
{
    public int Id { get; set; }

    public int Quantity { get; set; }
    public MenuItemResponse MenuItem { get; set; } = null!;

    public List<OrderItemOptionResponse> Options { get; set; } = [];

    public string MenuItemNameSnapshotAtOrder { get; set; } = null!;
    public string? MenuItemDescriptionAtOrder { get; set; }
    public decimal MenuItemBasePriceSnapshotAtOrder { get; set; }
    public string MenuItemImageUrlSnapshotAtOrder { get; set; } = null!;

    public decimal TotalPrice { get; set; }
}
