namespace OutpatientClinic.Api.Features.Appointments.BookAppointment;

public record BookAppointmentResponse(
    Guid AppointmentId,
    Guid PatientId,
    Guid DoctorId,
    Guid SlotId,
    DateTime ScheduledAtUtc,
    string Status);
