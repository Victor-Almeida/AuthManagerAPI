using System.Security.Claims;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AuthManager.Application.Admin.AssignRoleToAccount;

internal sealed class AssignRoleToAccountCommandHandler : IRequestHandler<AssignRoleToAccountCommand, OperationResult>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public AssignRoleToAccountCommandHandler(RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<OperationResult> Handle(AssignRoleToAccountCommand command, CancellationToken cancellationToken)
    {
        OperationResult result = new();

        var role = await _roleManager.FindByIdAsync(command.RoleId.ToString());
        var user = await _userManager.FindByIdAsync(command.AccountId.ToString());

        if (role is null)
            result.AddValidationErrorMessage("Role not found");

        if (user is null)
            result.AddValidationErrorMessage("Account not found");

        if (result.IsNotValid)
            return result;

        IdentityResult roleAssignmentResult = await _userManager.AddToRoleAsync(user!, role!.Name!);

        if (roleAssignmentResult.Succeeded is false) 
            return result.AddValidationErrorMessages(roleAssignmentResult.Errors.Select(x => x.Description));

        Claim claim = new(ClaimTypes.Role, role!.Name!);

        IdentityResult claimAssignmentResult = await _userManager.AddClaimAsync(user!, claim);

        if (claimAssignmentResult.Succeeded is false)
        {
            await _userManager.RemoveFromRoleAsync(user!, role.Name!);
            return result.AddValidationErrorMessages(claimAssignmentResult.Errors.Select(x => x.Description));
        }

        return result;
    }
}
