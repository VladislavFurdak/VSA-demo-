using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Prescriptions.CreatePrescription;

public static class CreatePrescriptionEndpoint
{
    public static IEndpointRouteBuilder MapCreatePrescription(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/prescriptions", HandleAsync)
            .WithName("CreatePrescription")
            .WithTags("Prescriptions")
            .WithSummary("Issue a prescription during a visit.")
            .Produces<CreatePrescriptionResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        CreatePrescriptionRequest request,
        IValidator<CreatePrescriptionRequest> validator,
        ICreatePrescriptionHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/prescriptions/{value.PrescriptionId}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
