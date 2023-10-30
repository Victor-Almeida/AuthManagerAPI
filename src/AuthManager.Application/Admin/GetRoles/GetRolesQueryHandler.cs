using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AuthManager.Domain.Extensions;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Admin.GetRoles;

/// <summary>
/// Handles the creation of a new role and adds it to the database.
/// </summary>
public sealed class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, OperationResult<PaginatedList<Role>>>
{
    private readonly RoleManager<Role> _roleManager;

    public GetRolesQueryHandler(
        RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Lists registered roles using pagination.
    /// </summary>
    /// <returns>
    /// An operation result containing the roles. 
    /// </returns>
    public async Task<OperationResult<PaginatedList<Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        OperationResult<PaginatedList<Role>> result = new();

        PaginatedList<Role> roles = await _roleManager.Roles.ToPaginatedListAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        result.Data = roles;

        return result;
    }
}
