using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Patients.DeactivatePatient;

public static class DeactivatePatientEndpoint
{
    public static IEndpointRouteBuilder MapDeactivatePatient(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/patients/{patientId:guid}/deactivate", HandleAsync)
            .WithName("DeactivatePatient")
            .WithTags("Patients")
            .WithSummary("Deactivate a patient.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid patientId,
        IDeactivatePatientHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(patientId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
