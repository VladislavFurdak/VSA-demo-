using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Appointments.BookAppointment;

public interface IBookAppointmentHandler
{
    Task<Result<BookAppointmentResponse>> HandleAsync(
        BookAppointmentRequest request,
        CancellationToken cancellationToken);
}

public class BookAppointmentHandler(
    AppDbContext dbContext,
    ILogger<BookAppointmentHandler> logger)
    : IBookAppointmentHandler
{
    public async Task<Result<BookAppointmentResponse>> HandleAsync(
        BookAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var patient = await dbContext.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == request.PatientId, cancellationToken);

        if (patient is null)
            return Result.Failure<BookAppointmentResponse>(PatientErrors.NotFound(request.PatientId));

        if (!patient.IsActive)
            return Result.Failure<BookAppointmentResponse>(PatientErrors.Deactivated(request.PatientId));

        var slot = await dbContext.AvailabilitySlots
            .FirstOrDefaultAsync(s => s.Id == request.SlotId && s.DoctorId == request.DoctorId, cancellationToken);

        if (slot is null)
            return Result.Failure<BookAppointmentResponse>(DoctorErrors.SlotNotFound(request.SlotId));

        if (!slot.IsAvailable)
            return Result.Failure<BookAppointmentResponse>(DoctorErrors.SlotNotAvailable(request.SlotId));

        if (!slot.IsFutureSlot(DateTime.UtcNow))
            return Result.Failure<BookAppointmentResponse>(DoctorErrors.SlotInThePast(request.SlotId));

        var hasActiveAppointment = await dbContext.Appointments
            .AnyAsync(a => a.SlotId == request.SlotId && (a.Status == AppointmentStatus.Created || a.Status == AppointmentStatus.Confirmed), cancellationToken);

        if (hasActiveAppointment)
            return Result.Failure<BookAppointmentResponse>(AppointmentErrors.SlotAlreadyBooked(request.SlotId));

        var scheduledAt = slot.Date.Date + slot.StartTime;
        var appointment = Appointment.Create(request.PatientId, request.DoctorId, request.SlotId, scheduledAt);

        slot.MarkBooked();
        dbContext.Appointments.Add(appointment);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Booked appointment {AppointmentId} for patient {PatientId} with doctor {DoctorId}",
            appointment.Id, request.PatientId, request.DoctorId);

        return Result.Success(new BookAppointmentResponse(
            appointment.Id,
            appointment.PatientId,
            appointment.DoctorId,
            appointment.SlotId,
            appointment.ScheduledAtUtc,
            appointment.Status.ToString()));
    }
}
