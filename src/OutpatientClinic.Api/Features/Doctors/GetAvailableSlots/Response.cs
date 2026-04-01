namespace OutpatientClinic.Api.Features.Doctors.GetAvailableSlots;

public record GetAvailableSlotsResponse(List<AvailableSlotDto> Slots);

public record AvailableSlotDto(
    Guid SlotId,
    DateTime Date,
    TimeSpan StartTime,
    TimeSpan EndTime);
