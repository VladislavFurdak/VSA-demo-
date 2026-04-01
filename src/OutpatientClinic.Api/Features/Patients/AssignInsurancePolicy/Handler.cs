using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Patients.AssignInsurancePolicy;

public interface IAssignInsurancePolicyHandler
{
    Task<Result<AssignInsurancePolicyResponse>> HandleAsync(
        Guid patientId,
        AssignInsurancePolicyRequest request,
        CancellationToken cancellationToken);
}

public class AssignInsurancePolicyHandler(AppDbContext dbContext)
    : IAssignInsurancePolicyHandler
{
    public async Task<Result<AssignInsurancePolicyResponse>> HandleAsync(
        Guid patientId,
        AssignInsurancePolicyRequest request,
        CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(x => x.Id == patientId, cancellationToken);

        if (patient is null)
            return Result.Failure<AssignInsurancePolicyResponse>(PatientErrors.NotFound(patientId));

        var policy = InsurancePolicy.Create(
            request.PolicyNumber,
            request.Provider,
            request.ValidFrom,
            request.ValidTo);

        patient.AssignInsurancePolicy(policy);
        await dbContext.SaveChangesAsync(cancellationToken);

        var paymentBasis = patient.DeterminePaymentBasis(DateTime.UtcNow);

        return Result.Success(new AssignInsurancePolicyResponse(
            patient.Id,
            policy.PolicyNumber,
            policy.Provider,
            policy.Status.ToString(),
            paymentBasis.ToString()));
    }
}
