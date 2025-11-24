namespace NileFood.Application.Contracts.Categories;
public class CategoryRequest
{
    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }
}
