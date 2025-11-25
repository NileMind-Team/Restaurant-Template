namespace NileFood.Application.Contracts.Branches;
public class BranchRequestValidator : AbstractValidator<BranchRequest>
{
    public BranchRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Branch name is required")
            .MaximumLength(200);


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");


        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(300);


        RuleFor(x => x.LocationUrl)
            .NotEmpty().WithMessage("Location URL is required")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Invalid location URL");


        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => s == "Open" || s == "Closed")
            .WithMessage("Status must be either 'Open' or 'Closed'");


        RuleFor(x => x.Rating_Avgarage)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(5)
            .WithMessage("Rating must be between 0 and 5.");


        RuleFor(x => x.OpeningTime)
            .Must(ts => ts >= TimeSpan.Zero && ts < TimeSpan.FromDays(1))
            .WithMessage("Opening time must be a valid time of 00:00");

        RuleFor(x => x.ClosingTime)
            .Must(ts => ts >= TimeSpan.Zero && ts < TimeSpan.FromDays(1))
            .WithMessage("Closing time must be a valid time of 00:00");

        RuleFor(x => x)
            .Must(x => x.OpeningTime < x.ClosingTime)
            .WithMessage("Opening time must be before closing time.");


        RuleFor(x => x.IsActive)
            .NotNull();


        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("CityId must be greater than 0");


        RuleFor(x => x.ManagerId)
            .NotEmpty().WithMessage("Manager Id is required");
    }
}
