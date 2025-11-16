using NileFood.Domain.Entities.Identity;

namespace NileFood.Application.Authentication;

public interface IJwtProvider
{
    public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles);
}