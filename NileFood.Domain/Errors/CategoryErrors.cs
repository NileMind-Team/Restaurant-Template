using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;

public class CategoryErrors
{
    public static readonly Error CategoryNotFound = new("Category.NotFound", "Category is not found ", StatusCodes.Status404NotFound);
}
