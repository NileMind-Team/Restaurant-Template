using NileFood.Application.Contracts.DeliveryFees;
using NileFood.Application.Contracts.Locations;

namespace NileFood.Application.Contracts.Orders;
public class OrderResponse
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public DeliveryFeeResponse DeliveryFee { get; set; } = null!;

    public LocationResponse Location { get; set; } = null!;



    public decimal TotalWithoutFee { get; set; }
    public decimal DeliveryCost { get; set; }
    public decimal TotalWithFee { get; set; }


    public string OrderNumber { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }


    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public List<OrderItemResponse> Items { get; set; } = [];
}
