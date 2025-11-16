namespace NileFood.Application.Contracts.Users.Validators;
public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName).Length(3, 100);
        RuleFor(x => x.LastName).Length(3, 100);

        RuleFor(s => s.PhoneNumber)
            .Matches(@"^(010|011|012|015)")
            .WithMessage("Invalid phone number, must start with 010, 011, 012, or 015.")
            .Length(11).WithMessage("Invalid phone number, must end with 11 numbers.");
    }
}
