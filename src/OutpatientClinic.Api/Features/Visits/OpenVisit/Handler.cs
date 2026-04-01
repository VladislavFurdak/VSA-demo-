using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Visits.OpenVisit;

public interface IOpenVisitHandler
{
    Task<Result<OpenVisitResponse>> HandleAsync(OpenVisitRequest request, CancellationToken cancellationToken);
}

public class OpenVisitHandler(
    AppDbContext dbContext,
    ILogger<OpenVisitHandler> logger)
    : IOpenVisitHandler
{
    public async Task<Result<OpenVisitResponse>> HandleAsync(
        OpenVisitRequest request,
        CancellationToken cancellationToken)
    {
        var appointment = await dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure<OpenVisitResponse>(AppointmentErrors.NotFound(request.AppointmentId));

        if (appointment.Status != AppointmentStatus.Confirmed)
            return Result.Failure<OpenVisitResponse>(AppointmentErrors.NotConfirmed(request.AppointmentId));

        var visitExists = await dbContext.Visits
            .AnyAsync(v => v.AppointmentId == request.AppointmentId, cancellationToken);

        if (visitExists)
            return Result.Failure<OpenVisitResponse>(AppointmentErrors.AlreadyHasVisit(request.AppointmentId));

        var visit = Visit.Open(appointment.Id, appointment.PatientId, appointment.DoctorId);

        dbContext.Visits.Add(visit);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Opened visit {VisitId} for appointment {AppointmentId}", visit.Id, appointment.Id);

        return Result.Success(new OpenVisitResponse(
            visit.Id,
            visit.AppointmentId,
            visit.PatientId,
            visit.DoctorId,
            visit.Status.ToString(),
            visit.OpenedAtUtc));
    }
}
