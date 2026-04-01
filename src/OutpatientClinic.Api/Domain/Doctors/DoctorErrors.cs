using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Domain.Doctors;

public static class DoctorErrors
{
    public static Error NotFound(Guid doctorId) =>
        Error.NotFound(
            code: DoctorErrorCodes.NotFound,
            message: $"Doctor '{doctorId}' was not found.");

    public static Error SlotNotFound(Guid slotId) =>
        Error.NotFound(
            code: DoctorErrorCodes.SlotNotFound,
            message: $"Availability slot '{slotId}' was not found.");

    public static Error SlotNotAvailable(Guid slotId) =>
        Error.Conflict(
            code: DoctorErrorCodes.SlotNotAvailable,
            message: $"Slot '{slotId}' is not available for booking.");

    public static Error SlotInThePast(Guid slotId) =>
        Error.BusinessRule(
            code: DoctorErrorCodes.SlotInThePast,
            message: $"Slot '{slotId}' is in the past and cannot be booked.");

    public static Error SlotOverlap() =>
        Error.Conflict(
            code: DoctorErrorCodes.SlotOverlap,
            message: "The slot overlaps with an existing availability slot.");

    public static Error SlotAlreadyBooked(Guid slotId) =>
        Error.Conflict(
            code: DoctorErrorCodes.SlotAlreadyBooked,
            message: $"Slot '{slotId}' is already booked.");
}
