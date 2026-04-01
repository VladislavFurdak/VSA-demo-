using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Appointments.ConfirmAppointment;

public static class ConfirmAppointmentEndpoint
{
    public static IEndpointRouteBuilder MapConfirmAppointment(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments/{appointmentId:guid}/confirm", HandleAsync)
            .WithName("ConfirmAppointment")
            .WithTags("Appointments")
            .WithSummary("Confirm an appointment.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid appointmentId,
        IConfirmAppointmentHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(appointmentId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
