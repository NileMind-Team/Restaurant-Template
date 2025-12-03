using NileFood.Application.Contracts.Users;

namespace NileFood.Application.Contracts.Reviews;
public class ReviewResponse
{
    public int Id { get; set; }

    public UserResponse User { get; set; } = null!;

    public int BranchId { get; set; }

    public decimal Rating { get; set; }
    public string? Comment { get; set; }
}
