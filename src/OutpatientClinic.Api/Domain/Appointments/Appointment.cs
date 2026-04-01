namespace OutpatientClinic.Api.Domain.Appointments;

public class Appointment
{
    public static readonly TimeSpan MinCancellationNotice = TimeSpan.FromHours(2);

    private Appointment() { }

    public Guid Id { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid DoctorId { get; private set; }
    public Guid SlotId { get; private set; }
    public DateTime ScheduledAtUtc { get; private set; }
    public AppointmentStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public string? CancellationReason { get; private set; }

    public bool IsActive => Status is AppointmentStatus.Created or AppointmentStatus.Confirmed;

    public static Appointment Create(
        Guid patientId,
        Guid doctorId,
        Guid slotId,
        DateTime scheduledAtUtc)
    {
        return new Appointment
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            DoctorId = doctorId,
            SlotId = slotId,
            ScheduledAtUtc = scheduledAtUtc,
            Status = AppointmentStatus.Created,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Confirm()
    {
        if (Status != AppointmentStatus.Created)
            throw new InvalidOperationException($"Cannot confirm appointment in status '{Status}'.");
        Status = AppointmentStatus.Confirmed;
    }

    public void CancelByPatient(string reason, DateTime utcNow)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Cannot cancel appointment in status '{Status}'.");

        if (ScheduledAtUtc - utcNow < MinCancellationNotice)
            throw new InvalidOperationException(
                $"Cannot cancel appointment less than {MinCancellationNotice.TotalHours} hours before the scheduled time.");

        Status = AppointmentStatus.CancelledByPatient;
        CancellationReason = reason;
    }

    public void CancelByClinic(string reason)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Cannot cancel appointment in status '{Status}'.");

        Status = AppointmentStatus.CancelledByClinic;
        CancellationReason = reason;
    }

    public void MarkCompleted()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new InvalidOperationException($"Cannot complete appointment in status '{Status}'.");
        Status = AppointmentStatus.Completed;
    }

    public void MarkNoShow()
    {
        if (Status != AppointmentStatus.Confirmed)
            throw new InvalidOperationException($"Cannot mark no-show for appointment in status '{Status}'.");
        Status = AppointmentStatus.NoShow;
    }

    public void Reschedule(Guid newSlotId, DateTime newScheduledAtUtc)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Cannot reschedule appointment in status '{Status}'.");

        SlotId = newSlotId;
        ScheduledAtUtc = newScheduledAtUtc;
        Status = AppointmentStatus.Created;
    }
}
