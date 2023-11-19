using MediatR;
using AuthManager.Application.Account.CreateAccount;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Primitives;
using Carter;
using Microsoft.AspNetCore.Authorization;

namespace AuthManager.WebAPI.Endpoints;

public class AccountEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/accounts")
            .WithTags("Accounts");

        group
            .MapPost(
                "",
                [AllowAnonymous]
                async (CreateAccountCommand command, ISender sender) => await CreateAccount(command, sender))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Create an account",
                Description = "Creates an account that allows the user to authenticate to the system"
            })
            .Accepts<CreateAccountCommand>("application/json")
            .Produces<AuthViewModel>(StatusCodes.Status200OK, "application/json")
            .Produces<OperationResult>(StatusCodes.Status400BadRequest)
            .Produces<OperationResult>(StatusCodes.Status500InternalServerError);
    }

    public async Task<IResult> CreateAccount(CreateAccountCommand command, ISender sender)
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
