namespace NileFood.Domain.Entities;
public class MenuItemOption
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public bool IsAvailableNow { get; set; } = true;
    public bool IsActive { get; set; } = true;

    public int MenuItemId { get; set; }
    public MenuItem MenuItem { get; set; } = null!;

    public int TypeId { get; set; }
    public MenuItemOptionType Type { get; set; } = null!;
}