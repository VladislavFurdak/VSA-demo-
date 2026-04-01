using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Prescriptions.CreatePrescription;

public interface ICreatePrescriptionHandler
{
    Task<Result<CreatePrescriptionResponse>> HandleAsync(
        CreatePrescriptionRequest request,
        CancellationToken cancellationToken);
}

public class CreatePrescriptionHandler(AppDbContext dbContext) : ICreatePrescriptionHandler
{
    public async Task<Result<CreatePrescriptionResponse>> HandleAsync(
        CreatePrescriptionRequest request,
        CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);

        if (visit is null)
            return Result.Failure<CreatePrescriptionResponse>(VisitErrors.NotFound(request.VisitId));

        if (visit.IsCompleted)
            return Result.Failure<CreatePrescriptionResponse>(VisitErrors.AlreadyCompleted(request.VisitId));

        var medications = request.Medications
            .Select(m => (m.MedicationName, m.Dosage, m.Frequency, m.Duration))
            .ToList();

        var prescription = Prescription.Create(request.VisitId, medications);

        dbContext.Prescriptions.Add(prescription);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreatePrescriptionResponse(
            prescription.Id,
            prescription.VisitId,
            prescription.IssuedAtUtc,
            prescription.Medications.Select(m => new MedicationItemResponse(
                m.Id,
                m.MedicationName,
                m.Dosage,
                m.Frequency,
                m.Duration)).ToList()));
    }
}
