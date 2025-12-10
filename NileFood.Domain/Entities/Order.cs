using NileFood.Domain.Entities.Identity;

namespace NileFood.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public int DeliveryFeeId { get; set; }
    public DeliveryFee DeliveryFee { get; set; } = null!;

    public int LocationId { get; set; }
    public Location Location { get; set; } = null!;


    public decimal TotalWithoutFee { get; set; }
    public decimal DeliveryCost { get; set; }
    public decimal TotalWithFee { get; set; }


    public string OrderNumber { get; set; } = null!;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? Notes { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }

    public List<OrderItem> Items { get; set; } = [];



    public void RecalculateTotals(decimal fee, decimal discount)
    {
        TotalWithoutFee = Items.Sum(i => i.TotalPrice) - discount;
        DeliveryCost = fee;
        TotalWithFee = TotalWithoutFee + DeliveryCost;
    }
}


public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Preparing = 2,
    OutForDelivery = 3,
    Delivered = 4,
    Cancelled = 5
}
