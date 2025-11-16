using Microsoft.Extensions.Options;
using NileFood.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace NileFood.Application.Authentication;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles)
    {
        Claim[] claims = [
            new(JwtRegisteredClaimNames.Sub,user.Id),
            new(JwtRegisteredClaimNames.Email,user.Email!),
            new(JwtRegisteredClaimNames.PhoneNumber,user.PhoneNumber!),
            new(JwtRegisteredClaimNames.Name,user.UserName),
            new(JwtRegisteredClaimNames.Jti,Guid.CreateVersion7().ToString()),
            new(nameof(roles),JsonSerializer.Serialize(roles),JsonClaimValueTypes.JsonArray),
        ];


        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // 
        var expiresIn = _jwtOptions.ExpiryYears;
        var expirationDate = DateTime.UtcNow.AddYears(expiresIn);


        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signingCredentials
        );


        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn);

    }
}