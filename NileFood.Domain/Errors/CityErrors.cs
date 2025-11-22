using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;

public class CityErrors
{
    public static readonly Error CityNotFound = new("City.NotFound", "City is not found ", StatusCodes.Status404NotFound);
    public static readonly Error CityNameAlreadyUsed = new("City.NameAlreadyUsed", "City name is already used.", StatusCodes.Status409Conflict);
}
