using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Patients.GetPatientById;

public interface IGetPatientByIdHandler
{
    Task<Result<GetPatientByIdResponse>> HandleAsync(Guid patientId, CancellationToken cancellationToken);
}

public class GetPatientByIdHandler(AppDbContext dbContext) : IGetPatientByIdHandler
{
    public async Task<Result<GetPatientByIdResponse>> HandleAsync(
        Guid patientId,
        CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == patientId, cancellationToken);

        if (patient is null)
            return Result.Failure<GetPatientByIdResponse>(PatientErrors.NotFound(patientId));

        var paymentBasis = patient.DeterminePaymentBasis(DateTime.UtcNow);

        InsurancePolicyDto? policyDto = null;
        if (patient.InsurancePolicy is not null)
        {
            policyDto = new InsurancePolicyDto(
                patient.InsurancePolicy.PolicyNumber,
                patient.InsurancePolicy.Provider,
                patient.InsurancePolicy.ValidFrom,
                patient.InsurancePolicy.ValidTo,
                patient.InsurancePolicy.Status.ToString());
        }

        return Result.Success(new GetPatientByIdResponse(
            patient.Id,
            patient.FullName,
            patient.FirstName,
            patient.LastName,
            patient.MiddleName,
            patient.DateOfBirth,
            patient.Contact.Phone,
            patient.Contact.Email,
            patient.IsActive,
            paymentBasis.ToString(),
            policyDto));
    }
}
