using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CompleteLaboratoryOrder;

public static class CompleteLaboratoryOrderEndpoint
{
    public static IEndpointRouteBuilder MapCompleteLaboratoryOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/laboratory-orders/{orderId:guid}/complete", HandleAsync)
            .WithName("CompleteLaboratoryOrder")
            .WithTags("Laboratory Orders")
            .WithSummary("Mark a laboratory order as completed.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid orderId,
        ICompleteLaboratoryOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(orderId, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
