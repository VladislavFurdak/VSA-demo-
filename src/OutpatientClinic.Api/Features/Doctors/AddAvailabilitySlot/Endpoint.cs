using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Doctors.AddAvailabilitySlot;

public static class AddAvailabilitySlotEndpoint
{
    public static IEndpointRouteBuilder MapAddAvailabilitySlot(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/doctors/{doctorId:guid}/slots", HandleAsync)
            .WithName("AddAvailabilitySlot")
            .WithTags("Doctors")
            .WithSummary("Add an availability slot for a doctor.")
            .Produces<AddAvailabilitySlotResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid doctorId,
        AddAvailabilitySlotRequest request,
        IValidator<AddAvailabilitySlotRequest> validator,
        IAddAvailabilitySlotHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(doctorId, request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/doctors/{doctorId}/slots/{value.SlotId}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
