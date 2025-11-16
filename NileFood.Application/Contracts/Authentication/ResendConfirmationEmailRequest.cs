using System.ComponentModel.DataAnnotations;

namespace NileFood.Application.Contracts.Authentication;
public record ResendConfirmationEmailRequest(
    [EmailAddress] string Email
);
