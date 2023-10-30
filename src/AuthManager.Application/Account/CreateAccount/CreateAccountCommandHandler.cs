using MediatR;
using Microsoft.AspNetCore.Identity;
using AuthManager.Application.Auth.AuthenticateUser;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Account.CreateAccount;

internal sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, OperationResult<AuthViewModel>>
{
    private readonly ISender _sender;
    private readonly UserManager<User> _userManager;

    public CreateAccountCommandHandler(
        ISender sender,
        UserManager<User> userManager)
    {
        _sender = sender;
        _userManager = userManager;
    }

    public async Task<OperationResult<AuthViewModel>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        OperationResult<AuthViewModel> result = new();

        User? user = await _userManager.FindByEmailAsync(command.Email);

        if (user is not null) return result.AddValidationErrorMessage("This e-mail is already in use.");

        user = new User()
        {
            Email = command.Email,
            NormalizedEmail = command.Email.ToLower(),
            NormalizedUserName = command.Username.ToLower(),
            UserName = command.Username
        };

        IdentityResult response = await _userManager.CreateAsync(user);

        if (response.Succeeded is false)
            return result.AddValidationErrorMessages(response.Errors.Select(x => x.Description));

        response = await _userManager.AddPasswordAsync(user, command.Password);

        if (response.Succeeded is false)
        {
            await _userManager.DeleteAsync(user);
            return result.AddValidationErrorMessages(response.Errors.Select(x => x.Description));
        }

        return await _sender.Send(new AuthenticateUserCommand(command.Email, command.Password), cancellationToken);
    }
}
