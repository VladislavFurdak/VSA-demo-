namespace OutpatientClinic.Api.Features.Appointments.RescheduleAppointment;

public record RescheduleAppointmentResponse(
    Guid AppointmentId,
    Guid NewSlotId,
    DateTime NewScheduledAtUtc,
    string Status);
