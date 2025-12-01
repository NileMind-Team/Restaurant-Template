using NileFood.Application.Contracts.MenuItems;

namespace NileFood.Application.Contracts.BranchMenuItemOptions;
public class OptionTypeWithOptions
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<MenuItemOptionResponse> MenuItemOptions { get; set; } = [];
}
