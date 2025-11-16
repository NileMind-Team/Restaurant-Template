using System.ComponentModel.DataAnnotations;

namespace NileFood.Application.Contracts.Authentication;
public record ForgetPasswordRequest(
    [EmailAddress] string Email
);