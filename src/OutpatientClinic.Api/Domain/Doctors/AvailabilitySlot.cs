namespace OutpatientClinic.Api.Domain.Doctors;

public class AvailabilitySlot
{
    private AvailabilitySlot() { }

    public Guid Id { get; private set; }
    public Guid DoctorId { get; private set; }
    public DateTime Date { get; private set; }
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public bool IsBlocked { get; private set; }
    public bool IsBooked { get; private set; }

    public bool IsAvailable => !IsBlocked && !IsBooked;

    public static AvailabilitySlot Create(Guid doctorId, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        return new AvailabilitySlot
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            IsBlocked = false,
            IsBooked = false
        };
    }

    public void Block()
    {
        if (IsBooked)
            throw new InvalidOperationException("Cannot block a slot that is already booked.");
        IsBlocked = true;
    }

    public void MarkBooked()
    {
        IsBooked = true;
    }

    public void ReleaseBooking()
    {
        IsBooked = false;
    }

    public bool IsFutureSlot(DateTime utcNow)
    {
        var slotDateTime = Date.Date + StartTime;
        return slotDateTime > utcNow;
    }
}
