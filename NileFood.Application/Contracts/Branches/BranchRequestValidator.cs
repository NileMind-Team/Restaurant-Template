using NileFood.Application.Contracts.PhoneNumbers;

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
            .Must(url => url == null || Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.Host.Contains("google.com") && uri.AbsolutePath.StartsWith("/maps"))
            .WithMessage("Invalid Google Maps URL.");



        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(s => s == "Open" || s == "Closed")
            .WithMessage("Status must be either 'Open' or 'Closed'");


        RuleForEach(x => x.PhoneNumbers).SetValidator(new PhoneNumberRequestValidator());

        RuleFor(x => x.PhoneNumbers)
        .Must(numbers => numbers == null
            || numbers.GroupBy(n => n.Phone).All(g => g.Count() == 1))
        .WithMessage("Phone numbers must be unique.");


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
