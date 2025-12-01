using NileFood.Application.Contracts.BranchMenuItemOptions;

namespace NileFood.Application.Contracts.MenuItemOptions;
public class MenuItemOptionResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public List<BranchMenuItemOptionResponse> BranchMenuItemOption { get; set; } = [];
}



