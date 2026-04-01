using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Patients.AssignInsurancePolicy;

public static class AssignInsurancePolicyEndpoint
{
    public static IEndpointRouteBuilder MapAssignInsurancePolicy(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/patients/{patientId:guid}/insurance", HandleAsync)
            .WithName("AssignInsurancePolicy")
            .WithTags("Patients")
            .WithSummary("Assign or change insurance policy for a patient.")
            .Produces<AssignInsurancePolicyResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid patientId,
        AssignInsurancePolicyRequest request,
        IValidator<AssignInsurancePolicyRequest> validator,
        IAssignInsurancePolicyHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(patientId, request, cancellationToken);

        return result.Match(
            onSuccess: value => TypedResults.Ok(value),
            onFailure: error => error.ToProblemHttpResult());
    }
}
