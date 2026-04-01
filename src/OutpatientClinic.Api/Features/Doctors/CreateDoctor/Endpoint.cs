using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Doctors.CreateDoctor;

public static class CreateDoctorEndpoint
{
    public static IEndpointRouteBuilder MapCreateDoctor(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/doctors", HandleAsync)
            .WithName("CreateDoctor")
            .WithTags("Doctors")
            .WithSummary("Register a new doctor.")
            .Produces<CreateDoctorResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        CreateDoctorRequest request,
        IValidator<CreateDoctorRequest> validator,
        ICreateDoctorHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Created($"/api/doctors/{value.Id}", value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
