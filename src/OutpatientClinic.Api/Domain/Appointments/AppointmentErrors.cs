using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Domain.Appointments;

public static class AppointmentErrors
{
    public static Error NotFound(Guid appointmentId) =>
        Error.NotFound(
            code: AppointmentErrorCodes.NotFound,
            message: $"Appointment '{appointmentId}' was not found.");

    public static Error SlotAlreadyBooked(Guid slotId) =>
        Error.Conflict(
            code: AppointmentErrorCodes.SlotAlreadyBooked,
            message: $"Slot '{slotId}' already has an active appointment.");

    public static Error InvalidStatusTransition(Guid appointmentId, string currentStatus, string targetStatus) =>
        Error.BusinessRule(
            code: AppointmentErrorCodes.InvalidStatusTransition,
            message: $"Cannot transition appointment '{appointmentId}' from '{currentStatus}' to '{targetStatus}'.");

    public static Error CancellationTooLate(Guid appointmentId) =>
        Error.BusinessRule(
            code: AppointmentErrorCodes.CancellationTooLate,
            message: $"Cannot cancel appointment '{appointmentId}' less than {Appointment.MinCancellationNotice.TotalHours} hours before the scheduled time.");

    public static Error NotConfirmed(Guid appointmentId) =>
        Error.BusinessRule(
            code: AppointmentErrorCodes.NotConfirmed,
            message: $"Appointment '{appointmentId}' is not confirmed.");

    public static Error AlreadyHasVisit(Guid appointmentId) =>
        Error.Conflict(
            code: AppointmentErrorCodes.AlreadyHasVisit,
            message: $"Appointment '{appointmentId}' already has an associated visit.");
}
