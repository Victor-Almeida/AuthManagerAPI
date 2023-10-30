using MediatR;
using AuthManager.Application.Admin.CreateRole;
using AuthManager.Application.Admin.GetRoles;
using AuthManager.Domain.Identity.Entities;
using AuthManager.Domain.Primitives;
using AuthManager.Application.Admin.AssignRoleToAccount;

namespace AuthManager.WebAPI.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/admin")
            .WithTags("Admin");

        group
            .MapPost("accounts/{accountId}/roles/{roleId}", async ([AsParameters] AssignRoleToAccountCommand command, ISender sender) => await AssignRoleToAccount(command, sender))
            .RequireAuthorization("Admin")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Assign role to account",
                Description = "Assigns a role to an existing acccount"
            })
            .Accepts<AssignRoleToAccountCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<OperationResult>(StatusCodes.Status400BadRequest)
            .Produces<OperationResult>(StatusCodes.Status500InternalServerError);

        group
            .MapPost("roles", async (CreateRoleCommand command, ISender sender) => await CreateRole(command, sender))
            .RequireAuthorization("Admin")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create a role",
                Description = "Creates a role that controls the functionalities each user can access"
            })
            .Accepts<CreateRoleCommand>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces<OperationResult>(StatusCodes.Status400BadRequest)
            .Produces<OperationResult>(StatusCodes.Status500InternalServerError);

        group
            .MapGet("roles", async ([AsParameters] GetRolesQuery query, ISender sender) => await GetRoles(query, sender))
            .RequireAuthorization("Admin")
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get roles",
                Description = "Lists all registered roles"
            })
            .Produces<PaginatedList<Role>>(StatusCodes.Status200OK, "application/json")
            .Produces<OperationResult>(StatusCodes.Status400BadRequest)
            .Produces<OperationResult>(StatusCodes.Status500InternalServerError);
    }

    public static async Task<IResult> AssignRoleToAccount(AssignRoleToAccountCommand command, ISender sender)
    {
        OperationResult operationResult;

        try
        {
            operationResult = await sender.Send(command);
        }
        catch (Exception ex)
        {
            operationResult = new OperationResult().AddInternalErrorMessage(ex.Message);

            return Results.Json(operationResult, statusCode: (int)operationResult.StatusCode);
        }

        if (operationResult.IsValid)
            return Results.Ok();

        return Results.BadRequest(operationResult);
    }

    public static async Task<IResult> CreateRole(CreateRoleCommand command, ISender sender)
    {
        OperationResult operationResult;

        try
        {
            operationResult = await sender.Send(command);
        }
        catch (Exception ex)
        {
            operationResult = new OperationResult().AddInternalErrorMessage(ex.Message);

            return Results.Json(operationResult, statusCode: (int)operationResult.StatusCode);
        }

        if (operationResult.IsValid)
            return Results.Ok();

        return Results.BadRequest(operationResult);
    }

    public static async Task<IResult> GetRoles(GetRolesQuery query, ISender sender)
    {
        OperationResult<PaginatedList<Role>> operationResult;

        try
        {
            operationResult = await sender.Send(query);
        }
        catch (Exception ex)
        {
            operationResult = new OperationResult<PaginatedList<Role>>().AddInternalErrorMessage(ex.Message);

            return Results.Json(operationResult, statusCode: (int)operationResult.StatusCode);
        }

        if (operationResult.IsValid)
            return Results.Ok(operationResult.Data);

        return Results.BadRequest(operationResult);
    }
}
