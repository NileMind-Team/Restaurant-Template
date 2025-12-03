using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class CartItemErrors
{
    public static readonly Error CartItemNotFound = new("CartItem.NotFound", "CartItem is not found ", StatusCodes.Status404NotFound);
}
