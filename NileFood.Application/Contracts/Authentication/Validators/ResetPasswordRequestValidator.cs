using NileFood.Domain.Consts;

namespace NileFood.Application.Contracts.Authentication.Validators;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Invalid code");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password).WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

    }
}
