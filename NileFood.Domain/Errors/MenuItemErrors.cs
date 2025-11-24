using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;

public class MenuItemErrors
{
    public static readonly Error MenuItemNotFound = new("MenuItem.NotFound", "MenuItem is not found ", StatusCodes.Status404NotFound);

    public static readonly Error InvalidMenuItemId = new("MenuItem.InvalidId", "Menu item id must be greater than zero", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidIds = new("MenuItemSchedule.InvalidIds", "These schedule IDs do not exist", StatusCodes.Status400BadRequest);
    public static readonly Error ScheduleIdNotAllowed =
    new("MenuItemSchedule.IdNotAllowed",
        "Schedule Ids must not be sent when creating a new menu item.",
        StatusCodes.Status400BadRequest);

}