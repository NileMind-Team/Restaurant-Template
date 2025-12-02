namespace NileFood.Application.Contracts.ItemOffers;
public class ItemOfferRequestValidator : AbstractValidator<ItemOfferRequest>
{
    public ItemOfferRequestValidator()
    {
        RuleFor(x => x.MenuItemId)
            .GreaterThan(0)
            .WithMessage("Menu item is required.");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0)
            .WithMessage("Discount value must be greater than 0.");

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.StartDate)
            .GreaterThan(DateTime.UtcNow.AddMinutes(-1))
            .WithMessage("Start date cannot be in the past.");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("End date must be in the future.");

        RuleFor(x => x.IsPercentage)
            .NotNull();

        When(x => x.IsPercentage, () =>
        {
            RuleFor(x => x.DiscountValue)
                .LessThanOrEqualTo(100)
                .WithMessage("Percentage discount cannot exceed 100.");
        });

        RuleFor(x => x.BranchesIds)
            .NotNull();


        RuleFor(x => x.BranchesIds)
            .Must(b => b.Any())
            .WithMessage("At least one branch is required.");

        // Unique branches
        RuleFor(x => x.BranchesIds)
            .Must(list => list.Distinct().Count() == list.Count)
            .WithMessage("Branch list contains duplicated values.");
    }
}
