using Microsoft.AspNetCore.Http;

namespace NileFood.Domain.Errors;
public class ReviewsErrors
{
    public static readonly Error ReviewNotFound = new("Review.NotFound", "Review is not found ", StatusCodes.Status404NotFound);
    public static readonly Error ReviewNameAlreadyUsed = new("Review.NameAlreadyUsed", "Review name is already used.", StatusCodes.Status409Conflict);
}
