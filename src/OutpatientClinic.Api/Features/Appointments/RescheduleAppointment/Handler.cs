using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Appointments.RescheduleAppointment;

public interface IRescheduleAppointmentHandler
{
    Task<Result<RescheduleAppointmentResponse>> HandleAsync(
        Guid appointmentId,
        RescheduleAppointmentRequest request,
        CancellationToken cancellationToken);
}

public class RescheduleAppointmentHandler(AppDbContext dbContext) : IRescheduleAppointmentHandler
{
    public async Task<Result<RescheduleAppointmentResponse>> HandleAsync(
        Guid appointmentId,
        RescheduleAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var appointment = await dbContext.Appointments
            .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure<RescheduleAppointmentResponse>(AppointmentErrors.NotFound(appointmentId));

        if (!appointment.IsActive)
            return Result.Failure<RescheduleAppointmentResponse>(
                AppointmentErrors.InvalidStatusTransition(appointmentId, appointment.Status.ToString(), "Rescheduled"));

        var newSlot = await dbContext.AvailabilitySlots
            .FirstOrDefaultAsync(s => s.Id == request.NewSlotId, cancellationToken);

        if (newSlot is null)
            return Result.Failure<RescheduleAppointmentResponse>(DoctorErrors.SlotNotFound(request.NewSlotId));

        if (!newSlot.IsAvailable)
            return Result.Failure<RescheduleAppointmentResponse>(DoctorErrors.SlotNotAvailable(request.NewSlotId));

        if (!newSlot.IsFutureSlot(DateTime.UtcNow))
            return Result.Failure<RescheduleAppointmentResponse>(DoctorErrors.SlotInThePast(request.NewSlotId));

        var oldSlot = await dbContext.AvailabilitySlots
            .FirstOrDefaultAsync(s => s.Id == appointment.SlotId, cancellationToken);

        oldSlot?.ReleaseBooking();
        newSlot.MarkBooked();

        var newScheduledAt = newSlot.Date.Date + newSlot.StartTime;
        appointment.Reschedule(request.NewSlotId, newScheduledAt);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new RescheduleAppointmentResponse(
            appointment.Id,
            request.NewSlotId,
            newScheduledAt,
            appointment.Status.ToString()));
    }
}
