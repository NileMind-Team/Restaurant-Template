namespace NileFood.Application.Contracts.Reviews;
public class ReviewRequest
{
    public int BranchId { get; set; }

    public decimal Rating { get; set; }
    public string Comment { get; set; } = null!;
}
