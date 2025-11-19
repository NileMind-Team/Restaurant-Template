using Microsoft.AspNetCore.Identity;

namespace NileFood.Domain.Entities.Identity;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int? DefaultLocationId { get; set; }
    public Location? DefaultLocation { get; set; }


    public List<Location> Locations { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
    public List<Branch> Branches { get; set; } = [];


}
