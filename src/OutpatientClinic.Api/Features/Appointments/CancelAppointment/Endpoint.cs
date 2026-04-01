using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Appointments.CancelAppointment;

public static class CancelAppointmentEndpoint
{
    public static IEndpointRouteBuilder MapCancelAppointment(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments/{appointmentId:guid}/cancel", HandleAsync)
            .WithName("CancelAppointment")
            .WithTags("Appointments")
            .WithSummary("Cancel an appointment.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid appointmentId,
        CancelAppointmentRequest request,
        IValidator<CancelAppointmentRequest> validator,
        ICancelAppointmentHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(appointmentId, request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
