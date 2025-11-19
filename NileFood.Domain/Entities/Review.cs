using NileFood.Domain.Entities.Identity;

namespace NileFood.Domain.Entities;
public class Review
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public decimal Rating { get; set; }
    public string? Comment { get; set; }
}
