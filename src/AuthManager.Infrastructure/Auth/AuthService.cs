using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AuthManager.Application.Interfaces;
using AuthManager.Domain.Identity.Entities;
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

    public string GenerateToken(User user, IEnumerable<string> userRoles)
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_authOptions.SecretKey));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
        };

        foreach (var role in userRoles)
        {
            var claim = new Claim(ClaimTypes.Role, role);
            claims.Add(claim);
        }

        JwtSecurityToken token = new(
            _authOptions.Issuer,
            _authOptions.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: credentials);

        string bearerToken = new JwtSecurityTokenHandler().WriteToken(token);

        return bearerToken;
    }
}