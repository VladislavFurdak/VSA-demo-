namespace OutpatientClinic.Api.Domain.Appointments;

public enum AppointmentStatus
{
    Created,
    Confirmed,
    CancelledByPatient,
    CancelledByClinic,
    Completed,
    NoShow
}
