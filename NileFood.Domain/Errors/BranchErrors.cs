using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class BranchErrors
{
    public static readonly Error BranchNotFound = new("Branch.NotFound", "Branch is not found ", StatusCodes.Status404NotFound);
    public static readonly Error BranchNameAlreadyUsed = new("Branch.NameAlreadyUsed", "Branch name is already used.", StatusCodes.Status409Conflict);
    public static readonly Error InvalidBranchId = new("Branch.InvalidId", "Menu item id must be greater than zero", StatusCodes.Status400BadRequest);
}
