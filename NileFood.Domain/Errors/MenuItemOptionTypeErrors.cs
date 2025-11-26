using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class MenuItemOptionTypeErrors
{
    public static readonly Error MenuItemOptionTypeNotFound = new("MenuItemOptionType.NotFound", "MenuItemOptionType is not found ", StatusCodes.Status404NotFound);
    public static readonly Error MenuItemOptionTypeNameAlreadyUsed = new("MenuItemOptionType.NameAlreadyUsed", "MenuItemOptionType name is already used.", StatusCodes.Status409Conflict);
}
