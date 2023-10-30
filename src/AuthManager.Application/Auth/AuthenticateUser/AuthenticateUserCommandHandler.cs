using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthManager.Application.Interfaces;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Auth.AuthenticateUser;

/// <summary>
/// Handles user authentication for MMO Party Finder.
/// </summary>
public sealed class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, OperationResult<AuthViewModel>>
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;

    public AuthenticateUserCommandHandler(
        IAuthService authService,
        UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    /// <summary>
    /// Authenticates users for MMO Party Finder.
    /// </summary>
    /// <returns>
    /// A result object containing the authentication token in case of success. Otherwise returns the result object with the validation errors. 
    /// </returns>
    public async Task<OperationResult<AuthViewModel>> Handle(AuthenticateUserCommand command, CancellationToken cancellationToken)
    {
        OperationResult<AuthViewModel> result = new();

        User? user = await _userManager.FindByEmailAsync(command.Email);

        if (user is null) 
            return result.AddValidationErrorMessage("User not found");

        bool passwordIsValid = await _userManager.CheckPasswordAsync(user, command.Password);

        if (passwordIsValid is false) 
            return result.AddValidationErrorMessage("Invalid password");

        IList<string> userRoles = await _userManager.GetRolesAsync(user);

        string authenticationToken = _authService.GenerateToken(user, userRoles);

        return result.SetData(new AuthViewModel(authenticationToken, user.Id, user.UserName!));
    }
}
