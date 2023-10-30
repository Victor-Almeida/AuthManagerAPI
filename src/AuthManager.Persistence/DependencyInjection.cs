using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Persistence.Contexts;

namespace AuthManager.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistence(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        services
            .AddDbContext<DefaultDbContext>(options => 
                options.UseLazyLoadingProxies()
                .UseSqlServer(
                    connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }))
            .AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddUserManager<UserManager<User>>()
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<DefaultDbContext>();

        return services;
    }
}
