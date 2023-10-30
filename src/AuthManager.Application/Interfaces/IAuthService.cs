using AuthManager.Domain.Identity.Entities;

namespace AuthManager.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(User user, IEnumerable<string> userRoles);
}