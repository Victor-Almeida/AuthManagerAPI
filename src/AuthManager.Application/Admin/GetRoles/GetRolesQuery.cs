using MediatR;
using AuthManager.Application.Filters;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;

namespace AuthManager.Application.Admin.GetRoles;

public record GetRolesQuery() : PaginationFilter, IRequest<OperationResult<PaginatedList<Role>>> { }
