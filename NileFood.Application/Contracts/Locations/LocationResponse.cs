using NileFood.Application.Contracts.Cities;

namespace NileFood.Application.Contracts.Locations;

public class LocationResponse
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public CityResponse City { get; set; } = null!;

    public string LocationUrl { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public int BuildingNumber { get; set; }
    public int FloorNumber { get; set; }
    public int FlatNumber { get; set; }


    public string? DetailedDescription { get; set; }    
    public bool IsDefaultLocation { get; set; }
}