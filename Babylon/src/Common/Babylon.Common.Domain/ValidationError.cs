namespace Babylon.Common.Domain;
public sealed class ValidationError : Error
{
    private ValidationError(string code, string description, ErrorType type, Dictionary<string, object>? metadata, List<Error> errors) : base(code, description, type, metadata)
    {
        Errors = errors;
    }

    public List<Error> Errors { get;}

    public static ValidationError Validation(List<string> errors, Dictionary<string, object>? metadata = null) {
        foreach(string error in errors){
            Errors.Add(new ValidationError("General.Validation", error, ErrorType.Validation, metadata));
        }
}
