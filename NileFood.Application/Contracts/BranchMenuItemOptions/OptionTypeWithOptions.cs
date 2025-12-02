using NileFood.Application.Contracts.MenuItemOptions;

namespace NileFood.Application.Contracts.BranchMenuItemOptions;
public class OptionTypeWithOptions
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool CanSelectMultipleOptions { get; set; }
    public bool IsSelectionRequired { get; set; }

    public List<MenuItemOptionResponse> MenuItemOptions { get; set; } = [];
}
