namespace NileFood.Application.Contracts.MenuItemOptionTypes;
public class MenuItemOptionTypeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public bool CanSelectMultipleOptions { get; set; }
    public bool IsSelectionRequired { get; set; }
}
