using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AuthManager.Domain.Enums;
using AuthManager.Application.Interfaces;

namespace AuthManager.Application.Account.CreateAccount;

internal sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, OperationResult<AuthViewModel>>
{
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;

    public CreateAccountCommandHandler(
        IAuthService authService,
        UserManager<User> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    public async Task<OperationResult<AuthViewModel>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        User? user = await _userManager.FindByEmailAsync(command.Email);

        if (user is not null) 
            return OperationResult<AuthViewModel>.Failure(FailureTypeEnum.Validation, "This e-mail is already in use.");

        user = new User()
        {
            Email = command.Email,
            NormalizedEmail = command.Email.ToLower(),
            NormalizedUserName = command.Username.ToLower(),
            UserName = command.Username
        };

        IdentityResult response = await _userManager.CreateAsync(user);

        if (response.Succeeded is false)
            return OperationResult<AuthViewModel>.Failure(FailureTypeEnum.Validation, response.Errors.Select(x => x.Description));

        response = await _userManager.AddPasswordAsync(user, command.Password);

        if (response.Succeeded is false)
        {
            await _userManager.DeleteAsync(user);
            return OperationResult<AuthViewModel>.Failure(FailureTypeEnum.Validation, response.Errors.Select(x => x.Description));
        }

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
        };

        response = await _userManager.AddClaimsAsync(user, claims);

        if (response.Succeeded is false)
        {
            await _userManager.DeleteAsync(user);
            return OperationResult<AuthViewModel>.Failure(FailureTypeEnum.Validation, response.Errors.Select(x => x.Description));
        }

        AuthViewModel viewModel = new(
            _authService.GenerateToken(claims),
            user.Id,
            user.UserName
        );

        return OperationResult<AuthViewModel>.Success(viewModel);
    }
}
