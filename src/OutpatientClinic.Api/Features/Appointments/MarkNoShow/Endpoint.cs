using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Appointments.MarkNoShow;

public static class MarkNoShowEndpoint
{
    public static IEndpointRouteBuilder MapMarkNoShow(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments/{appointmentId:guid}/no-show", HandleAsync)
            .WithName("MarkNoShow")
            .WithTags("Appointments")
            .WithSummary("Mark a patient as not attended.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid appointmentId,
        IMarkNoShowHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(appointmentId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
