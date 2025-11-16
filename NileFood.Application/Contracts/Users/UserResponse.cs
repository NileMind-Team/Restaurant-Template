namespace NileFood.Application.Contracts.Users;

public class UserResponse
{
    public string Id { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string PhoneNumber { get; init; } = null!;
    public string ImageUrl { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = [];
}
