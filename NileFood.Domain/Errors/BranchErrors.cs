using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class BranchErrors
{
    public static readonly Error BranchNotFound = new("Branch.NotFound", "Branch is not found ", StatusCodes.Status404NotFound);
    public static readonly Error BranchNameAlreadyUsed = new("Branch.NameAlreadyUsed", "Branch name is already used.", StatusCodes.Status409Conflict);
    public static readonly Error InvalidBranchId = new("Branch.InvalidId", "Menu item id must be greater than zero", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidIds = new("PhoneNumber.InvalidIds", "cannot send phone number id when create", StatusCodes.Status400BadRequest);
    public static readonly Error NoBranchesForManager =
    new("MenuItem.NoBranchesForManager",
        "This manager does not have any assigned branches.",
        StatusCodes.Status400BadRequest);
}
