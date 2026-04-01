using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Patients.GetPatientById;

public static class GetPatientByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetPatientById(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/patients/{patientId:guid}", HandleAsync)
            .WithName("GetPatientById")
            .WithTags("Patients")
            .WithSummary("Get patient details by ID.")
            .Produces<GetPatientByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid patientId,
        IGetPatientByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(patientId, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Ok(value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
