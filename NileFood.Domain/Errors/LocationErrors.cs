using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;

public class LocationErrors
{
    public static readonly Error LocationNotFound = new("Location.NotFound", "Location is not found ", StatusCodes.Status404NotFound);
}
