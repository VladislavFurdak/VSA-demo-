using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Patients.UpdatePatientContacts;

public interface IUpdatePatientContactsHandler
{
    Task<Result> HandleAsync(Guid patientId, UpdatePatientContactsRequest request, CancellationToken cancellationToken);
}

public class UpdatePatientContactsHandler(AppDbContext dbContext)
    : IUpdatePatientContactsHandler
{
    public async Task<Result> HandleAsync(
        Guid patientId,
        UpdatePatientContactsRequest request,
        CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .FirstOrDefaultAsync(x => x.Id == patientId, cancellationToken);

        if (patient is null)
            return Result.Failure(PatientErrors.NotFound(patientId));

        patient.UpdateContacts(request.Phone, request.Email);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
