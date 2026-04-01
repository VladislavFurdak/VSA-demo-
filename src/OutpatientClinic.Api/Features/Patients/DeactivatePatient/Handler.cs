using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Patients.DeactivatePatient;

public interface IDeactivatePatientHandler
{
    Task<Result> HandleAsync(Guid patientId, CancellationToken cancellationToken);
}

public class DeactivatePatientHandler(
    AppDbContext dbContext,
    ILogger<DeactivatePatientHandler> logger)
    : IDeactivatePatientHandler
{
    public async Task<Result> HandleAsync(Guid patientId, CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(x => x.Id == patientId, cancellationToken);

        if (patient is null)
            return Result.Failure(PatientErrors.NotFound(patientId));

        if (!patient.IsActive)
            return Result.Failure(PatientErrors.AlreadyDeactivated(patientId));

        patient.Deactivate();
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Deactivated patient {PatientId}", patientId);

        return Result.Success();
    }
}
