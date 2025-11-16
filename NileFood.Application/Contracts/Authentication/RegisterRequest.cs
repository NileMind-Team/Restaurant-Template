namespace NileFood.Application.Contracts.Authentication;
public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Password,
    string ConfirmPassword
);


//public class RegisterRequest
//{
//    public string Name { get; set; } = null!;
//    public string Email { get; set; } = null!;
//    public string PhoneNumber { get; set; } = null!;
//    public string Password { get; set; } = null!;
//    public string ConfirmPassword { get; set; } = null!;
//}