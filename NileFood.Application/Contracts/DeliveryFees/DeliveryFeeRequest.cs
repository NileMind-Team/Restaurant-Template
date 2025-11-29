namespace NileFood.Application.Contracts.DeliveryFees;
public class DeliveryFeeRequest
{
    public int BranchId { get; set; }
    public string AreaName { get; set; } = null!;
    public decimal Fee { get; set; }

    public int EstimatedTimeMin { get; set; }
    public int EstimatedTimeMax { get; set; }
}
