using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class DeliveryFeeErrors
{
    public static readonly Error DeliveryFeeNotFound = new("DeliveryFee.NotFound", "DeliveryFee is not found ", StatusCodes.Status404NotFound);
    public static readonly Error DeliveryFeeNameAlreadyUsed = new("DeliveryFee.NameAlreadyUsed", "DeliveryFee name is already used.", StatusCodes.Status409Conflict);
    public static readonly Error DeliveryFeeAlreadyExists = new("DeliveryFee.AlreadyExists", "A delivery fee for this area already exists for this branch.", StatusCodes.Status409Conflict);

}
