namespace NileFood.Application.Contracts.MenuItemOptionTypes;
public class MenuItemOptionTypeRequest
{
    public string Name { get; set; } = null!;

    public bool CanSelectMultipleOptions { get; set; }
    public bool IsSelectionRequired { get; set; }
}
