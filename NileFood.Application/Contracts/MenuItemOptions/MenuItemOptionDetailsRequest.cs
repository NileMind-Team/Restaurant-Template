namespace NileFood.Application.Contracts.MenuItemOptions;
public class MenuItemOptionDetailsRequest
{
    public int MenuItemId { get; set; }
    public int TypeId { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
