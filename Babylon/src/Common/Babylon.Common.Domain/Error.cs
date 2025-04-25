
namespace Babylon.Common.Domain;
public class Error
{
    protected Error(string code, string description, ErrorType type, Dictionary<string, object>? metadata)
    {
        Code = code;
        Description = description;
        Type = type;
        NumericType = (int)type;
        Metadata = metadata;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorType Type { get; }
    public int NumericType { get; }
    public Dictionary<string, object>? Metadata { get; }

    public static Error Failure(
        string code = "General.Failure",
        string description = "A failure has occured",
        Dictionary<string, object>? metadata = null
        ) =>  new (code, description, ErrorType.Failure, metadata);
    public static Error Validation(
        string code = "General.Validation",
        string description = "A validation error has occurred",
        Dictionary<string, object>? metadata = null
        ) => new(code, description, ErrorType.Validation, metadata);
    public static Error Unexpected(
        string code = "General.Unexpected",
        string description = "An unexpected error has occurred",
        Dictionary<string, object>? metadata = null
        ) => new(code, description,ErrorType.Unexpected, metadata);
    public static Error Conflict(
        string code ="General.Conflict",
        string description = "A conflict error has occurred",
        Dictionary<string, object>? metadata = null
        ) => new(code, description, ErrorType.Confilct, metadata);
    public static Error NotFound(
        string code = "General.NotFound",
        string description = "A not found error has occurred",
        Dictionary<string, object>? metadata = null
        ) => new(code, description, ErrorType.NotFound, metadata);
    public static Error Unauthorized(
        string code = "General.Unauthorized",
        string description = "An unauthorized error has occurred",
        Dictionary<string, object>? metadata = null
        ) => new(code, description, ErrorType.Unauthorized, metadata);
    public static Error Forbidden(
        string code = "General.Forbidden",
        string description = "A forbidden error has occurred",
        Dictionary<string, object>? metadata = null
        ) => new(code, description, ErrorType.Forbbiden, metadata);
    public static Error NullValue(
        string code = "General.Null",
        string description = "Null value was provided",
        Dictionary<string, object>? metadata = null
        ) => new(code, description, ErrorType.NullValue, metadata);

}
