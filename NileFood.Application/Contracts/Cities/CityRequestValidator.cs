using NileFood.Infrastructure.Data;

namespace NileFood.Application.Contracts.Cities;

public class CityRequestValidator : AbstractValidator<CityRequest>
{
    public CityRequestValidator(ApplicationDbContext context)
    {
        RuleFor(x => x.Name)
        .Must(name => !context.Cities.Any(c => c.Name == name))
        .WithMessage(name => $"name is already used.")
        .NotEmpty();

    }
}
