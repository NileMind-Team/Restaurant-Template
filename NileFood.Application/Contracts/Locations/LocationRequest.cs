namespace NileFood.Application.Contracts.Locations;

public class LocationRequest
{
    
    public int CityId { get; set; }    

    public string LocationUrl { get; set; } = null!;
    public string StreetName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int BuildingNumber { get; set; }
    public int FloorNumber { get; set; }
    public int FlatNumber { get; set; }

    public string? DetailedDescription { get; set; }    
}
