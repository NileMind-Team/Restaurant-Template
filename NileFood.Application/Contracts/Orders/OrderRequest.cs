namespace NileFood.Application.Contracts.Orders;
public class OrderRequest
{
    public int CartId { get; set; }
    public int BranchId { get; set; }
    public int DeliveryFeeId { get; set; }
    public decimal Discount { get; set; }

    public string? Notes { get; set; }
}