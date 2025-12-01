namespace NileFood.Domain.Entities;
public class BranchMenuItem
{
    public int Id { get; set; }

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = null!;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public bool IsAvailable { get; set; } = true;

    public List<BranchMenuItemOption> BranchMenuItemOptions { get; set; } = [];
}