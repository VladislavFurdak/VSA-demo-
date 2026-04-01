using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Patients.CreatePatient;

public static class CreatePatientEndpoint
{
    public static IEndpointRouteBuilder MapCreatePatient(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/patients", HandleAsync)
            .WithName("CreatePatient")
            .WithTags("Patients")
            .WithSummary("Register a new patient.")
            .Produces<CreatePatientResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        CreatePatientRequest request,
        IValidator<CreatePatientRequest> validator,
        ICreatePatientHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/patients/{value.Id}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
