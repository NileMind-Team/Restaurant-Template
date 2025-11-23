namespace NileFood.Application.Contracts.Categories;

public class CategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
