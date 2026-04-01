namespace OutpatientClinic.Api.Features.Doctors.AddAvailabilitySlot;

public record AddAvailabilitySlotRequest(
    DateTime Date,
    TimeSpan StartTime,
    TimeSpan EndTime);
