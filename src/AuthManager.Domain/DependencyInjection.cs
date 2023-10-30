using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AuthManager.Domain;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureDomain(this IServiceCollection services)
    {
        if (services.Any(x => x.ServiceType == typeof(IMediator)) is false)
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}
