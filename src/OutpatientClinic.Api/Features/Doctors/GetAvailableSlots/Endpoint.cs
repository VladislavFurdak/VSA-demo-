using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Doctors.GetAvailableSlots;

public static class GetAvailableSlotsEndpoint
{
    public static IEndpointRouteBuilder MapGetAvailableSlots(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/doctors/{doctorId:guid}/slots", HandleAsync)
            .WithName("GetAvailableSlots")
            .WithTags("Doctors")
            .WithSummary("Get available slots for a doctor.")
            .Produces<GetAvailableSlotsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid doctorId,
        DateTime? date,
        IGetAvailableSlotsHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(doctorId, date, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Ok(value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
