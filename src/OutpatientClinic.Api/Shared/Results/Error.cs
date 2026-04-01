namespace OutpatientClinic.Api.Shared.Results;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Unauthorized,
    Forbidden,
    BusinessRule,
    Failure
}

public record Error(
    ErrorType Type,
    string Code,
    string Message,
    IReadOnlyDictionary<string, string[]>? ValidationErrors = null)
{
    public static Error Validation(
        string code,
        string message,
        IReadOnlyDictionary<string, string[]> errors) =>
        new(ErrorType.Validation, code, message, errors);

    public static Error NotFound(string code, string message) =>
        new(ErrorType.NotFound, code, message);

    public static Error Conflict(string code, string message) =>
        new(ErrorType.Conflict, code, message);

    public static Error Unauthorized(string code, string message) =>
        new(ErrorType.Unauthorized, code, message);

    public static Error Forbidden(string code, string message) =>
        new(ErrorType.Forbidden, code, message);

    public static Error BusinessRule(string code, string message) =>
        new(ErrorType.BusinessRule, code, message);

    public static Error Failure(string code, string message) =>
        new(ErrorType.Failure, code, message);
}
