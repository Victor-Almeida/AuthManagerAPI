using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Admin.CreateRole;

/// <summary>
/// Handles the creation of a new role and adds it to the database.
/// </summary>
public sealed class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, OperationResult>
{
    private readonly RoleManager<Role> _roleManager;

    public CreateRoleCommandHandler(
        RoleManager<Role> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Creates a new role and adds it to the database after validation.
    /// </summary>
    /// <returns>
    /// An operation result containing validation errors, if any. 
    /// </returns>
    public async Task<OperationResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        OperationResult result = new();

        Role? role = await _roleManager.FindByNameAsync(request.RoleName);

        if (role is not null) return result.AddValidationErrorMessage("This role already exists.");

        role = new Role()
        {
            Name = request.RoleName,
            NormalizedName = request.RoleName.ToLower()
        };

        IdentityResult response = await _roleManager.CreateAsync(role);

        if (response.Succeeded is false)
            return result.AddValidationErrorMessages(response.Errors.Select(x => x.Description));

        return result;
    }
}
