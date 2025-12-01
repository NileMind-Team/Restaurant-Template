namespace NileFood.Application.Contracts.PhoneNumbers;
public class PhoneNumberRequestValidator : AbstractValidator<PhoneNumberRequest>
{
    public PhoneNumberRequestValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{8,15}$")
            .WithMessage("Invalid phone number format.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Phone type is required.")
            .Must(t => t == "Mobile" || t == "Landline" || t == "Other")
            .WithMessage("Phone type must be Mobile, Landline, or Other.");
    }
}
