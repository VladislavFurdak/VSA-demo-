using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Visits.CompleteVisit;

public static class CompleteVisitEndpoint
{
    public static IEndpointRouteBuilder MapCompleteVisit(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/visits/{visitId:guid}/complete", HandleAsync)
            .WithName("CompleteVisit")
            .WithTags("Visits")
            .WithSummary("Complete a visit.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid visitId,
        ICompleteVisitHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(visitId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
