namespace NileFood.Domain.Entities;
public class MenuItemOptionType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public List<MenuItemOption> Options { get; set; } = [];
}