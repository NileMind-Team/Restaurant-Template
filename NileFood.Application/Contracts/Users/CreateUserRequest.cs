namespace NileFood.Application.Contracts.Users;
public record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    IList<string> Roles
);

