namespace OutpatientClinic.Api.Features.Appointments.BookAppointment;

public record BookAppointmentRequest(
    Guid PatientId,
    Guid DoctorId,
    Guid SlotId);
