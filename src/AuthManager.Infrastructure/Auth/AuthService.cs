using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthManager.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthManager.Infrastructure.Auth;

internal sealed class AuthService : IAuthService
{
    private readonly AuthOptions _authOptions;

    public AuthService(IOptions<AuthOptions> authOptions)
    {
        _authOptions = authOptions.Value;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_authOptions.SecretKey));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _authOptions.Audience,
            Expires = DateTime.UtcNow.AddMinutes(60),
            IssuedAt = DateTime.UtcNow,
            Issuer = _authOptions.Issuer,
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims)            
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var bearerToken = tokenHandler.WriteToken(token);

        return bearerToken;
    }
}