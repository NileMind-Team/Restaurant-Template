namespace NileFood.Application.Contracts.MenuItemOptions;
public class MenuItemOptionDetailsRequestValidator : AbstractValidator<MenuItemOptionDetailsRequest>
{
    public MenuItemOptionDetailsRequestValidator()
    {
        RuleFor(x => x.MenuItemId)
            .GreaterThan(0)
            .WithMessage("MenuItemId must be greater than zero.");

        RuleFor(x => x.TypeId)
            .GreaterThan(0)
            .WithMessage("TypeId must be greater than zero.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Option name is required.")
            .MaximumLength(100).WithMessage("Option name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be equal or greater than zero.");
    }
}
