namespace NileFood.Domain.Entities;
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<MenuItem> MenuItems { get; set; } = [];
}
