using MediatR;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Auth.AuthenticateUser;

public record AuthenticateUserCommand(
    string Email, 
    string Password) : IRequest<OperationResult<AuthViewModel>>;