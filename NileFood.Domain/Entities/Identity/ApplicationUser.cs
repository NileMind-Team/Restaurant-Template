using Microsoft.AspNetCore.Identity;

namespace NileFood.Domain.Entities.Identity;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;
}
