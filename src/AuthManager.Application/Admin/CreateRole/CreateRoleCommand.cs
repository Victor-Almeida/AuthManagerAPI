using MediatR;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Admin.CreateRole;

public record CreateRoleCommand(string RoleName) : IRequest<OperationResult>;
