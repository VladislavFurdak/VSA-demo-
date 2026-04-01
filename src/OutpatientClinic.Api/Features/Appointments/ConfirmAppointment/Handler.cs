using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Appointments.ConfirmAppointment;

public interface IConfirmAppointmentHandler
{
    Task<Result> HandleAsync(Guid appointmentId, CancellationToken cancellationToken);
}

public class ConfirmAppointmentHandler(AppDbContext dbContext) : IConfirmAppointmentHandler
{
    public async Task<Result> HandleAsync(Guid appointmentId, CancellationToken cancellationToken)
    {
        var appointment = await dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure(AppointmentErrors.NotFound(appointmentId));

        if (appointment.Status != AppointmentStatus.Created)
            return Result.Failure(AppointmentErrors.InvalidStatusTransition(
                appointmentId, appointment.Status.ToString(), AppointmentStatus.Confirmed.ToString()));

        appointment.Confirm();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
