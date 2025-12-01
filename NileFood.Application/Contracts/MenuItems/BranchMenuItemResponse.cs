namespace NileFood.Application.Contracts.MenuItems;
public class BranchMenuItemResponse
{
    public int Id { get; set; }
    public int BranchId { get; set; }
    public bool IsAvailable { get; set; }
}