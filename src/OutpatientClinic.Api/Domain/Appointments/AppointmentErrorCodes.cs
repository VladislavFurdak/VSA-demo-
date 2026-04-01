namespace OutpatientClinic.Api.Domain.Appointments;

public static class AppointmentErrorCodes
{
    public const string NotFound = "appointments.not_found";
    public const string SlotAlreadyBooked = "appointments.slot_already_booked";
    public const string InvalidStatusTransition = "appointments.invalid_status_transition";
    public const string CancellationTooLate = "appointments.cancellation_too_late";
    public const string NotConfirmed = "appointments.not_confirmed";
    public const string AlreadyHasVisit = "appointments.already_has_visit";
}
