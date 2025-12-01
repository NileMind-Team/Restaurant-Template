using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class MenuItemOptionErrors
{
    public static readonly Error MenuItemOptionNotFound =
        new("MenuItemOption.NotFound", "Menu item option not found.", StatusCodes.Status404NotFound);

    public static readonly Error MenuItemOptionTypeNotFound =
        new("MenuItemOptionType.NotFound", "Menu item option type not found.", StatusCodes.Status404NotFound);

    public static readonly Error MenuItemOptionAlreadyExists =
        new("MenuItemOption.AlreadyExists", "Menu item option already exists.", StatusCodes.Status409Conflict);

    public static readonly Error CannotBeUpdatedDueToMultipleBranches =
    new("MenuItemOption.CannotBeUpdatedDueToMultipleBranches",
        "This menu item option cannot be updated because it is used in multiple branches.",
        StatusCodes.Status409Conflict);


    public static readonly Error NotManagerOfThisBranch =
    new("MenuItemOption.NotManagerOfThisBranch",
        "You are not allowed to update this menu item option because you are not the manager of its branch.",
        StatusCodes.Status403Forbidden);
}
