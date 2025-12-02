using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class ItemOfferErrors
{
    public static readonly Error ItemOfferNotFound = new("ItemOffer.NotFound", "ItemOffer is not found ", StatusCodes.Status404NotFound);
    public static readonly Error ItemOfferAlreadyExists = new("ItemOffer.ItemOfferAlreadyExists", "An active offer for this menu item already exists.", StatusCodes.Status409Conflict);

}
