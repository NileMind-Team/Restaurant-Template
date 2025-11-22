using NileFood.Infrastructure.Data;

namespace NileFood.Application.Contracts.Locations;

public class LocationRequestValidator : AbstractValidator<LocationRequest>
{
    
    public LocationRequestValidator(ApplicationDbContext context)
    {

        RuleFor(x => x.CityId)
            .GreaterThan(0)
            .Must((id) => context.Cities.Any(c => c.Id == id))
            .WithMessage("City not found.");

        RuleFor(x => x.LocationUrl)
            .NotEmpty()
            .Must(url => url.StartsWith("https://www.google.com/maps"))
            .WithMessage("Invalid Google Maps URL.");

        RuleFor(x => x.StreetName).NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^(010|011|012|015)")
            .WithMessage("Invalid phone number, must start with 010, 011, 012, or 015.")
            .Length(11)
            .WithMessage("Invalid phone number, must be 11 digits long.");

        RuleFor(x => x.BuildingNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FloorNumber).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FlatNumber).GreaterThanOrEqualTo(0);
    }

}
