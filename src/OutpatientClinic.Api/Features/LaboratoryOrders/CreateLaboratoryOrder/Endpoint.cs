using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CreateLaboratoryOrder;

public static class CreateLaboratoryOrderEndpoint
{
    public static IEndpointRouteBuilder MapCreateLaboratoryOrder(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/laboratory-orders", HandleAsync)
            .WithName("CreateLaboratoryOrder")
            .WithTags("Laboratory Orders")
            .WithSummary("Create a laboratory order during a visit.")
            .Produces<CreateLaboratoryOrderResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        CreateLaboratoryOrderRequest request,
        IValidator<CreateLaboratoryOrderRequest> validator,
        ICreateLaboratoryOrderHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/laboratory-orders/{value.OrderId}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
