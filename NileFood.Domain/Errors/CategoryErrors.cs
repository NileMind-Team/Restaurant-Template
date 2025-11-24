using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;

public class CategoryErrors
{
    public static readonly Error CategoryNotFound = new("Category.NotFound", "Category is not found ", StatusCodes.Status404NotFound);
    public static readonly Error CategoryNameAlreadyUsed = new("Category.NameAlreadyUsed", "Category name is already used.", StatusCodes.Status409Conflict);
    public static readonly Error InvalidCategoryId = new("Category.InvalidId", "Category id must be greater than zero", StatusCodes.Status400BadRequest);
}
