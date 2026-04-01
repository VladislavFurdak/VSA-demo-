using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Doctors.BlockAvailabilitySlot;

public static class BlockAvailabilitySlotEndpoint
{
    public static IEndpointRouteBuilder MapBlockAvailabilitySlot(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/doctors/{doctorId:guid}/slots/{slotId:guid}/block", HandleAsync)
            .WithName("BlockAvailabilitySlot")
            .WithTags("Doctors")
            .WithSummary("Block an availability slot.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid doctorId,
        Guid slotId,
        IBlockAvailabilitySlotHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(doctorId, slotId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
