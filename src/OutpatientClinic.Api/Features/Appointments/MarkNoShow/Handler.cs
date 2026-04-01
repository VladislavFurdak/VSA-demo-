using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Appointments.MarkNoShow;

public interface IMarkNoShowHandler
{
    Task<Result> HandleAsync(Guid appointmentId, CancellationToken cancellationToken);
}

public class MarkNoShowHandler(AppDbContext dbContext) : IMarkNoShowHandler
{
    public async Task<Result> HandleAsync(Guid appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure(AppointmentErrors.NotFound(appointmentId));

        if (appointment.Status != AppointmentStatus.Confirmed)
            return Result.Failure(AppointmentErrors.NotConfirmed(appointmentId));

        appointment.MarkNoShow();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
