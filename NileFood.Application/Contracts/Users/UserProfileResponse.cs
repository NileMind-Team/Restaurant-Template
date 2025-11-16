namespace NileFood.Application.Contracts.Users;

public record UserProfileResponse
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public IList<string> Roles { get; set; } = [];
}
