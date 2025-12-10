namespace NileFood.Application.Contracts.CartItems;
public class CartItemQuantityRequestValidator : AbstractValidator<CartItemQuantityRequest>
{
    public CartItemQuantityRequestValidator()
    {
        RuleFor(x => x.MenuItemId)
            .GreaterThan(0).WithMessage("MenuItemId must be greater than 0.");
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
