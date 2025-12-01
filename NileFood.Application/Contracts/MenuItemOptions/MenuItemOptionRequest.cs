namespace NileFood.Application.Contracts.MenuItemOptions;
public class MenuItemOptionRequest
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public bool IsAvailableNow { get; set; }
    public bool IsActive { get; set; }

    public int TypeId { get; set; }
}
