using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;

public class MenuItemErrors
{
    public static readonly Error MenuItemNotFound = new("MenuItem.NotFound", "MenuItem is not found ", StatusCodes.Status404NotFound);

    public static readonly Error InvalidMenuItemId = new("MenuItem.InvalidId", "Menu item id must be greater than zero", StatusCodes.Status400BadRequest);
}