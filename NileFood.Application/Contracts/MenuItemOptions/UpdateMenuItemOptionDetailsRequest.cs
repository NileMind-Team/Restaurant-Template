namespace NileFood.Application.Contracts.MenuItemOptions;

public class UpdateMenuItemOptionDetailsRequest
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public int TypeId { get; set; }
}
