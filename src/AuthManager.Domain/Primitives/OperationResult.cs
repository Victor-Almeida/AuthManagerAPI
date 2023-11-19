using System.Net;
using System.Text.Json.Serialization;
using AuthManager.Domain.Enums;

namespace AuthManager.Domain.Primitives;

/// <summary>
/// Represents an operation result. By default, the result is considered valid until either an error message or validation error is set.
/// </summary>
public class OperationResult
{
    /// <summary>
    /// Indicates whether the operation was not successful. 
    /// </summary>
    [JsonIgnore]
    public bool IsNotValid => ErrorMessages.Any();

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    [JsonIgnore]
    public bool IsValid => ErrorMessages.Any() is false;

    /// <summary>
    /// The reasons why the operation was not successful.
    /// </summary>
    public IList<string> ErrorMessages { get; protected set; }

    /// <summary>
    /// The HTTP status code to be returned by the API.
    /// </summary>
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; protected set; }

    public OperationResult()
    {
        ErrorMessages = new List<string>();
        StatusCode = HttpStatusCode.OK;
    }

    public static OperationResult Success() => new();

    public static OperationResult Failure(FailureTypeEnum failureType, string message)
    {
        var result = new OperationResult();

        return failureType switch
        {
            FailureTypeEnum.Validation => result.AddValidationErrorMessage(message),
            _ => result.AddInternalErrorMessage(message),
        };
    }

    public static OperationResult Failure(FailureTypeEnum failureType, IEnumerable<string> messages)
    {
        var result = new OperationResult();

        return failureType switch
        {
            FailureTypeEnum.Validation => result.AddValidationErrorMessages(messages),
            _ => result.AddInternalErrorMessages(messages),
        };
    }

    /// <summary>
    /// Adds an internal error message to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public virtual OperationResult AddInternalErrorMessage(string errorMessage)
    {
        ChangeStatusCode(HttpStatusCode.InternalServerError);
        ErrorMessages.Add(errorMessage);
        return this;
    }

    /// <summary>
    /// Adds several internal error messages to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public virtual OperationResult AddInternalErrorMessages(IEnumerable<string> errorMessages)
    {
        foreach (var errorMessage in errorMessages)
            AddInternalErrorMessage(errorMessage);

        return this;
    }

    /// <summary>
    /// Adds a validation error message to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public virtual OperationResult AddValidationErrorMessage(string errorMessage)
    {
        ChangeStatusCode(HttpStatusCode.BadRequest);
        ErrorMessages.Add(errorMessage);
        return this;
    }

    /// <summary>
    /// Adds several validation error messages to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public virtual OperationResult AddValidationErrorMessages(IEnumerable<string> errorMessages)
    {
        foreach (var errorMessage in errorMessages)
            AddValidationErrorMessage(errorMessage);

        return this;
    }

    /// <summary>
    /// Changes the status code to be returned by the API depending on the error type.
    /// </summary>
    protected void ChangeStatusCode(HttpStatusCode statusCode)
    {
        if (StatusCode == HttpStatusCode.InternalServerError || StatusCode == statusCode) return;

        StatusCode = statusCode;
    }
}

/// <summary>
/// Represents a typed operation result. By default, the result is considered valid until either an error message or validation error is set.
/// </summary>
public class OperationResult<T> : OperationResult
{
    /// <summary>
    /// The result of an operation.
    /// </summary>
    public T? Data { get; set; }

    public OperationResult() : base() { }

    public OperationResult(T data) : base()
    {
        Data = data;
    }

    private static new OperationResult Success() => new();

    public static OperationResult<T> Success(T data) => new(data);

    /// <summary>
    /// Adds an internal error message to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public override OperationResult<T> AddInternalErrorMessage(string errorMessage)
    {
        ChangeStatusCode(HttpStatusCode.InternalServerError);
        ErrorMessages.Add(errorMessage);
        return this;
    }

    /// <summary>
    /// Adds several internal error messages to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public override OperationResult<T> AddInternalErrorMessages(IEnumerable<string> errorMessages)
    {
        foreach (var errorMessage in errorMessages)
            AddInternalErrorMessage(errorMessage);

        return this;
    }

    /// <summary>
    /// Adds a validation error message to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public override OperationResult<T> AddValidationErrorMessage(string errorMessage)
    {
        ChangeStatusCode(HttpStatusCode.BadRequest);
        ErrorMessages.Add(errorMessage);
        return this;
    }

    /// <summary>
    /// Adds several validation error messages to the result in case of an unsuccessful operation. Makes the result invalid.
    /// </summary>
    public override OperationResult<T> AddValidationErrorMessages(IEnumerable<string> errorMessages)
    {
        foreach (var errorMessage in errorMessages)
            AddValidationErrorMessage(errorMessage);

        return this;
    }

    /// <summary>
    /// A monad method to set the result data for practicity purposes.
    /// </summary>
    public OperationResult<T> SetData(T data)
    {
        Data = data;

        return this;
    }
}
