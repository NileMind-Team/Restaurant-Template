using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class OrderErrors
{
    public static readonly Error NotFound = new("Order.NotFound", "Order is not found ", StatusCodes.Status404NotFound);
}
