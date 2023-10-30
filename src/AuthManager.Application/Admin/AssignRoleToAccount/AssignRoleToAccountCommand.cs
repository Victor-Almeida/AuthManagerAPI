using AuthManager.Domain.Primitives;
using MediatR;

namespace AuthManager.Application.Admin.AssignRoleToAccount;

public record AssignRoleToAccountCommand(Guid AccountId, Guid RoleId) : IRequest<OperationResult>;
