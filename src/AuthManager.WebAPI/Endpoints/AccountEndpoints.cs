using MediatR;
using AuthManager.Application.Account.CreateAccount;
using AuthManager.Application.ViewModels;
using AuthManager.Domain.Primitives;

namespace AuthManager.WebAPI.Endpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/accounts")
            .WithTags("Accounts");

        group
            .MapPost("", async (CreateAccountCommand command, ISender sender) => await CreateAccount(command, sender))
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

    public static async Task<IResult> CreateAccount(CreateAccountCommand command, ISender sender)
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
