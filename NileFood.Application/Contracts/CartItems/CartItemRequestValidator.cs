namespace NileFood.Application.Contracts.CartItems;
public class CartItemRequestValidator : AbstractValidator<CartItemRequest>
{
    public CartItemRequestValidator()
    {
        RuleFor(x => x.MenuItemId)
            .GreaterThan(0)
            .WithMessage("MenuItemId must be greater than 0.");


        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be at least 1.");


        RuleFor(x => x.Options)
        .NotNull().WithMessage("Options list cannot be null.")
        .Must(options => options.Distinct().Count() == options.Count).WithMessage("Duplicate option IDs are not allowed.");


        RuleForEach(x => x.Options)
            .GreaterThan(0)
            .WithMessage("Option ID must be greater than 0.");
    }
}
