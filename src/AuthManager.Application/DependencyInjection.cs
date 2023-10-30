using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AuthManager.Application.Account.CreateAccount;
using AuthManager.Application.Admin.CreateRole;
using AuthManager.Application.Admin.GetRoles;
using AuthManager.Application.Auth.AuthenticateUser;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;
using AuthManager.Application.Admin.AssignRoleToAccount;

namespace AuthManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services)
    {
        return services
            .AddTransient<IRequestHandler<AssignRoleToAccountCommand, OperationResult>, AssignRoleToAccountCommandHandler>()
            .AddTransient<IRequestHandler<AuthenticateUserCommand, OperationResult<AuthViewModel>>, AuthenticateUserCommandHandler>()
            .AddTransient<IRequestHandler<CreateAccountCommand, OperationResult<AuthViewModel>>, CreateAccountCommandHandler>()
            .AddTransient<IRequestHandler<CreateRoleCommand, OperationResult>, CreateRoleCommandHandler>()
            .AddTransient<IRequestHandler<GetRolesQuery, OperationResult<PaginatedList<Role>>>, GetRolesQueryHandler>();
    }
}
