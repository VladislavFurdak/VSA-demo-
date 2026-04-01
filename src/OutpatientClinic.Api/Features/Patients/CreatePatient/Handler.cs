using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Patients.CreatePatient;

public interface ICreatePatientHandler
{
    Task<Result<CreatePatientResponse>> HandleAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken);
}

public class CreatePatientHandler(
    AppDbContext dbContext,
    ILogger<CreatePatientHandler> logger)
    : ICreatePatientHandler
{
    public async Task<Result<CreatePatientResponse>> HandleAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken)
    {
        var patient = Patient.Create(
            request.FirstName,
            request.LastName,
            request.MiddleName,
            request.DateOfBirth,
            request.Phone,
            request.Email);

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created patient {PatientId} ({FullName})", patient.Id, patient.FullName);

        return Result.Success(new CreatePatientResponse(
            patient.Id,
            patient.FullName,
            patient.DateOfBirth,
            patient.Contact.Phone,
            patient.Contact.Email,
            patient.DeterminePaymentBasis(DateTime.UtcNow).ToString()));
    }
}
