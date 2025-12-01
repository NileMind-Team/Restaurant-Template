using NileFood.Domain.Entities.Identity;

namespace NileFood.Domain.Entities;
public class Location
{
    public int Id { get; set; }

    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public string? LocationUrl { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string StreetName { get; set; } = null!;

    public int BuildingNumber { get; set; }
    public int FloorNumber { get; set; }
    public int FlatNumber { get; set; }

    public string? DetailedDescription { get; set; }



    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
