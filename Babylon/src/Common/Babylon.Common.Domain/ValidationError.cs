using System.Collections.Generic;

namespace Babylon.Common.Domain;
public sealed class ValidationError : Error
{
    private ValidationError(IEnumerable<Error> errors, Dictionary<string, object>? metadata) : base("General.Validation", "One or more validation errors occurred", ErrorType.Validation, metadata)
    {
        Errors = errors;
    }

    public IEnumerable<Error> Errors { get;}

    public static ValidationError FromResults(IEnumerable<Result> errors, Dictionary<string, object>? metadata = null)
    {
        foreach (Result error in errors)
        {
            if (error.IsSuccess || error.Error is null || error.Error.Type != ErrorType.Validation)
            {
                throw new InvalidOperationException("All errors must be of validation type");
            }
        }

        return new(errors.Select(x => x.Error!), metadata);
    }
}
