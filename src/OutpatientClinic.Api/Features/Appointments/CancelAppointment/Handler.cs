using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Appointments.CancelAppointment;

public interface ICancelAppointmentHandler
{
    Task<Result> HandleAsync(Guid appointmentId, CancelAppointmentRequest request, CancellationToken cancellationToken);
}

public class CancelAppointmentHandler(AppDbContext dbContext) : ICancelAppointmentHandler
{
    public async Task<Result> HandleAsync(
        Guid appointmentId,
        CancelAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var appointment = await dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure(AppointmentErrors.NotFound(appointmentId));

        if (!appointment.IsActive)
            return Result.Failure(AppointmentErrors.InvalidStatusTransition(
                appointmentId, appointment.Status.ToString(), "Cancelled"));

        if (request.CancelledBy == "Patient")
        {
            if (appointment.ScheduledAtUtc - DateTime.UtcNow < Appointment.MinCancellationNotice)
                return Result.Failure(AppointmentErrors.CancellationTooLate(appointmentId));

            appointment.CancelByPatient(request.Reason, DateTime.UtcNow);
        }
        else
        {
            appointment.CancelByClinic(request.Reason);
        }

        var slot = await dbContext.AvailabilitySlots
            .FirstOrDefaultAsync(s => s.Id == appointment.SlotId, cancellationToken);

        slot?.ReleaseBooking();

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
