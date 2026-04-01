namespace OutpatientClinic.Api.Domain.Doctors;

public static class DoctorErrorCodes
{
    public const string NotFound = "doctors.not_found";
    public const string SlotNotFound = "doctors.slot_not_found";
    public const string SlotNotAvailable = "doctors.slot_not_available";
    public const string SlotInThePast = "doctors.slot_in_the_past";
    public const string SlotOverlap = "doctors.slot_overlap";
    public const string SlotAlreadyBooked = "doctors.slot_already_booked";
}
