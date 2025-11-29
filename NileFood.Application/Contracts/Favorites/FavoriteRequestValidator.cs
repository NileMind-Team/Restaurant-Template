namespace NileFood.Application.Contracts.Favorites;
public class FavoriteRequestValidator : AbstractValidator<FavoriteRequest>
{
    public FavoriteRequestValidator()
    {
        RuleFor(x => x.MenuItemId)
            .GreaterThan(0).WithMessage("MenuItemId must be greater than 0.");
    }
}
