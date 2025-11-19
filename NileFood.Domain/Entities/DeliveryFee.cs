namespace NileFood.Domain.Entities;
public class DeliveryFee
{
    public int Id { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public decimal Fee { get; set; }
}
