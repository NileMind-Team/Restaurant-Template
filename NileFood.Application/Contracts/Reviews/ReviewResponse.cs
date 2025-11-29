namespace NileFood.Application.Contracts.Reviews;
public class ReviewResponse
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public int BranchId { get; set; }

    public decimal Rating { get; set; }
    public string? Comment { get; set; }
}
