namespace OutpatientClinic.Api.Features.Appointments.CancelAppointment;

public record CancelAppointmentRequest(
    string Reason,
    string CancelledBy);
