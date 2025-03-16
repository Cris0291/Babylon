namespace Babylon.Common.Domain;
public partial class Result
{
    public Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    public Error? Error { get; }
    public bool IsSuccess { get; }
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
    public static Result<T> Success<T>(T TValue) => new(TValue, true, null);
    public static Result<T> Failure<T>(Error error) => new(default, false, error);

}

public partial class Result<T> : Result
{
    public Result(T? value, bool isSuccess, Error? error) : base(isSuccess, error)
    {
        TValue = value;
    }
    public T? TValue { get; }
    public static Result<T> ValidationFailure(Error error) => error.Type == ErrorType.Validation ? new(default, false, error) : throw new InvalidOperationException("ValidationFaiilure factory must be used only with validation errors"); 
}
