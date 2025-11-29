namespace NileFood.Application.Contracts.DeliveryFees;
public class DeliveryFeeRequestValidator : AbstractValidator<DeliveryFeeRequest>
{
    public DeliveryFeeRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("BranchId must be greater than 0.");

        RuleFor(x => x.AreaName)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Area name is required and must not exceed 200 characters.");

        RuleFor(x => x.Fee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Fee must be 0 or more.");

        RuleFor(x => x.EstimatedTimeMin)
            .GreaterThan(0)
            .WithMessage("Minimum estimated time must be greater than 0.");

        RuleFor(x => x.EstimatedTimeMax)
            .GreaterThan(x => x.EstimatedTimeMin)
            .WithMessage("Maximum estimated time must be greater than minimum estimated time.");
    }
}
