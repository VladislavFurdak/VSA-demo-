using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Visits.RecordVisitDetails;

public static class RecordVisitDetailsEndpoint
{
    public static IEndpointRouteBuilder MapRecordVisitDetails(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/visits/{visitId:guid}/details", HandleAsync)
            .WithName("RecordVisitDetails")
            .WithTags("Visits")
            .WithSummary("Record complaints, diagnosis, and recommendations for a visit.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid visitId,
        RecordVisitDetailsRequest request,
        IValidator<RecordVisitDetailsRequest> validator,
        IRecordVisitDetailsHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(visitId, request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
