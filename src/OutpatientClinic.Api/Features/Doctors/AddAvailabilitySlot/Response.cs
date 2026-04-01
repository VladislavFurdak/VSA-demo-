namespace OutpatientClinic.Api.Features.Doctors.AddAvailabilitySlot;

public record AddAvailabilitySlotResponse(
    Guid SlotId,
    Guid DoctorId,
    DateTime Date,
    TimeSpan StartTime,
    TimeSpan EndTime);
