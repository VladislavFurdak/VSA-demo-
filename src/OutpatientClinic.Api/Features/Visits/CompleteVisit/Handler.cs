using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Visits.CompleteVisit;

public interface ICompleteVisitHandler
{
    Task<Result> HandleAsync(Guid visitId, CancellationToken cancellationToken);
}

public class CompleteVisitHandler(AppDbContext dbContext, ILogger<CompleteVisitHandler> logger) : ICompleteVisitHandler
{
    public async Task<Result> HandleAsync(Guid visitId, CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == visitId, cancellationToken);

        if (visit is null)
            return Result.Failure(VisitErrors.NotFound(visitId));

        if (visit.IsCompleted)
            return Result.Failure(VisitErrors.AlreadyCompleted(visitId));

        if (string.IsNullOrWhiteSpace(visit.Diagnosis) && string.IsNullOrWhiteSpace(visit.Recommendations))
            return Result.Failure(VisitErrors.MissingMedicalOutcome(visitId));

        visit.Complete();

        var appointment = await dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == visit.AppointmentId, cancellationToken);

        if (appointment is not null && appointment.Status == AppointmentStatus.Confirmed)
        {
            appointment.MarkCompleted();
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Completed visit {VisitId} for appointment {AppointmentId}", visitId, visit.AppointmentId);

        return Result.Success();
    }
}
