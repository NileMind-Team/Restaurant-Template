namespace NileFood.Application.Contracts.BranchMenuItemOptions;
public class BranchMenuItemOptionResponse
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
}
