using FluentValidation;
using OutpatientClinic.Api.Shared.Http;

namespace OutpatientClinic.Api.Features.Patients.UpdatePatientContacts;

public static class UpdatePatientContactsEndpoint
{
    public static IEndpointRouteBuilder MapUpdatePatientContacts(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/patients/{patientId:guid}/contacts", HandleAsync)
            .WithName("UpdatePatientContacts")
            .WithTags("Patients")
            .WithSummary("Update patient contact details.")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> HandleAsync(
        Guid patientId,
        UpdatePatientContactsRequest request,
        IValidator<UpdatePatientContactsRequest> validator,
        IUpdatePatientContactsHandler handler,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            return TypedResults.ValidationProblem(validation.ToDictionary());
        }

        var result = await handler.HandleAsync(patientId, request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.NoContent()
            : result.Error.ToProblemHttpResult();
    }
}
