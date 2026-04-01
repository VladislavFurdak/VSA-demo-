namespace OutpatientClinic.Api.Domain.Doctors;

public class Doctor
{
    private readonly List<AvailabilitySlot> _availabilitySlots = [];

    private Doctor() { }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Specialization { get; private set; } = string.Empty;
    public IReadOnlyList<AvailabilitySlot> AvailabilitySlots => _availabilitySlots.AsReadOnly();

    public string FullName => $"{LastName} {FirstName}";

    public static Doctor Create(string firstName, string lastName, string specialization)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));
        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Specialization is required.", nameof(specialization));

        return new Doctor
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Specialization = specialization.Trim()
        };
    }

    public AvailabilitySlot AddAvailabilitySlot(DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time.");

        var hasOverlap = _availabilitySlots.Any(s =>
            s.Date == date
            && s.StartTime < endTime
            && s.EndTime > startTime);

        if (hasOverlap)
            throw new InvalidOperationException("The slot overlaps with an existing availability slot.");

        var slot = AvailabilitySlot.Create(Id, date, startTime, endTime);
        _availabilitySlots.Add(slot);
        return slot;
    }
}
