using NileFood.Domain.Consts;
using NileFood.Domain.Errors;
using NileFood.Infrastructure.Data;

namespace NileFood.Application.Contracts.Authentication.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private readonly ApplicationDbContext _applicationDbContext;
    public RegisterRequestValidator(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        var applicationDbContext1 = applicationDbContext;

        RuleFor(x => x.FirstName).NotEmpty().Length(3, 100);
        RuleFor(x => x.LastName).NotEmpty().Length(3, 100);

        RuleFor(x => x.Email).NotEmpty().EmailAddress()
            .Must(email => !applicationDbContext1.Users.Any(n => n.Email == email)).WithMessage(UserErrors.DuplicatedEmail.Description);


        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^(010|011|012|015)")
            .WithMessage("Invalid phone number, must start with 010, 011, 012, or 015.")
            .Length(11)
            .WithMessage("Invalid phone number, must be 11 digits long.")
            .Must(IsUniquePhoneNumber)
            .WithMessage("This phone number is already registered.");


        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password).WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match. Please make sure both password fields are identical");

    }

    private bool IsUniquePhoneNumber(string phoneNumber)
    {
        return !_applicationDbContext.Users.Any(u => u.PhoneNumber == phoneNumber);
    }

}
