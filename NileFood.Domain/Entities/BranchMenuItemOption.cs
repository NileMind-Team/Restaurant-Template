namespace NileFood.Domain.Entities;
public class BranchMenuItemOption
{
    public int Id { get; set; }

    public int BranchMenuItemId { get; set; }
    public BranchMenuItem BranchMenuItem { get; set; } = null!;

    public int MenuItemOptionId { get; set; }
    public MenuItemOption MenuItemOption { get; set; } = null!;

    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
}
