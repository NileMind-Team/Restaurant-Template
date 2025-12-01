namespace NileFood.Domain.Entities;
public class DeliveryFee
{
    public int Id { get; set; }

    public string AreaName { get; set; } = null!;
    public decimal Fee { get; set; }

    public int EstimatedTimeMin { get; set; }
    public int EstimatedTimeMax { get; set; }
    public bool IsActive { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
