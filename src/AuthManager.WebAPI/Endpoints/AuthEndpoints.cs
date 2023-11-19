using MediatR;
using AuthManager.Application.Auth.AuthenticateUser;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Primitives;
using Carter;
using Microsoft.AspNetCore.Authorization;

namespace AuthManager.WebAPI.Endpoints;

public class AuthEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/auth")
            .WithTags("Auth");

        group
            .MapPost(
                "login",
                [AllowAnonymous]
                async (AuthenticateUserCommand command, ISender sender) => await Login(command, sender))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Authenticate user",
                Description = "Authenticates a registered user, allowing them to access restricted areas of the system according to their role"
            })
            .Accepts<AuthenticateUserCommand>("application/json")
            .Produces<AuthViewModel>(StatusCodes.Status200OK, "application/json")
            .Produces<OperationResult>(StatusCodes.Status400BadRequest)
            .Produces<OperationResult>(StatusCodes.Status500InternalServerError);
    }

    public async Task<IResult> Login(AuthenticateUserCommand command, ISender sender)
    {
        OperationResult<AuthViewModel> operationResult;

        try
        {
            operationResult = await sender.Send(command);
        }
        catch (Exception ex)
        {
            operationResult = new OperationResult<AuthViewModel>().AddInternalErrorMessage(ex.Message);

            return Results.Json(operationResult, contentType: "application/json", statusCode: (int)operationResult.StatusCode);
        }

        if (operationResult.IsValid)
            return Results.Ok(operationResult.Data);

        return Results.BadRequest(operationResult);
    }
}
