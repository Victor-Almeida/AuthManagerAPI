using Microsoft.Extensions.DependencyInjection;
using AuthManager.Application.Interfaces;
using AuthManager.Infrastructure.Auth;

namespace AuthManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services)
    {
        return services
            .AddTransient<IAuthService, AuthService>();
    }
}