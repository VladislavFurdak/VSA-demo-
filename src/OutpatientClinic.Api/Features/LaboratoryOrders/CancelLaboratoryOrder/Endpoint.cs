using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CancelLaboratoryOrder;

public static class CancelLaboratoryOrderEndpoint
{
    public static IEndpointRouteBuilder MapCancelLaboratoryOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/laboratory-orders/{orderId:guid}/cancel", HandleAsync)
            .WithName("CancelLaboratoryOrder")
            .WithTags("Laboratory Orders")
            .WithSummary("Cancel a laboratory order.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid orderId,
        ICancelLaboratoryOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(orderId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
