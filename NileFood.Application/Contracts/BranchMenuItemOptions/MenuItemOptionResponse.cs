namespace NileFood.Application.Contracts.BranchMenuItemOptions;
public class MenuItemOptionResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public List<BranchMenuItemOptionResponse> BranchMenuItemOption { get; set; } = [];
}



