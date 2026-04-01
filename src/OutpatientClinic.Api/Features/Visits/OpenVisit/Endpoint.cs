using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Visits.OpenVisit;

public static class OpenVisitEndpoint
{
    public static IEndpointRouteBuilder MapOpenVisit(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/visits", HandleAsync)
            .WithName("OpenVisit")
            .WithTags("Visits")
            .WithSummary("Open a visit for a confirmed appointment.")
            .Produces<OpenVisitResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        OpenVisitRequest request,
        IValidator<OpenVisitRequest> validator,
        IOpenVisitHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/visits/{value.VisitId}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
