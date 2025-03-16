namespace Babylon.Common.Domain;
public partial class Result
{
    public static implicit operator Result(Error error) => Failure(error);
}

public partial class Result<T> : Result
{
    public static implicit operator Result<T>(T TValue) => TValue is not null ? Success<T>(TValue) : Failure<T>(Error.NullValue());
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
}
