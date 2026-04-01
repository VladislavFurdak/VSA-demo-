using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Appointments.BookAppointment;

public static class BookAppointmentEndpoint
{
    public static IEndpointRouteBuilder MapBookAppointment(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/appointments", HandleAsync)
            .WithName("BookAppointment")
            .WithTags("Appointments")
            .WithSummary("Book an appointment for a patient.")
            .Produces<BookAppointmentResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        BookAppointmentRequest request,
        IValidator<BookAppointmentRequest> validator,
        IBookAppointmentHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/appointments/{value.AppointmentId}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
