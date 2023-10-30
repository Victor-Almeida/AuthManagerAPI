using MediatR;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Account.CreateAccount
{
    public record CreateAccountCommand(
        string Email,
        string Password,
        string Username) : IRequest<OperationResult<AuthViewModel>>;
}
