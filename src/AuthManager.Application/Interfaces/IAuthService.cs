using System.Security.Claims;

namespace AuthManager.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(IEnumerable<Claim> claims);
}