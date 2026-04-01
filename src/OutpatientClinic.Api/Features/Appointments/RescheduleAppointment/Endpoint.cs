using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Appointments.RescheduleAppointment;

public static class RescheduleAppointmentEndpoint
{
    public static IEndpointRouteBuilder MapRescheduleAppointment(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments/{appointmentId:guid}/reschedule", HandleAsync)
            .WithName("RescheduleAppointment")
            .WithTags("Appointments")
            .WithSummary("Reschedule an appointment to a different slot.")
            .Produces<RescheduleAppointmentResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid appointmentId,
        RescheduleAppointmentRequest request,
        IValidator<RescheduleAppointmentRequest> validator,
        IRescheduleAppointmentHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(appointmentId, request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Ok(value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
