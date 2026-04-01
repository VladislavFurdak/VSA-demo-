using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Shared.Http;

public static class ErrorHttpMapping
{
    public static IResult ToProblemHttpResult(this Error error)
    {
        if (error.Type is ErrorType.Validation && error.ValidationErrors is not null)
        {
            return TypedResults.ValidationProblem(error.ValidationErrors);
        }

        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.BusinessRule => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(
            title: error.Type.ToString(),
            detail: error.Message,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = error.Code
            });
    }
}
