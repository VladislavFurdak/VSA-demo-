namespace OutpatientClinic.Api.Domain.Visits;

public class MedicationItem
{
    private MedicationItem() { }

    public Guid Id { get; private set; }
    public Guid PrescriptionId { get; private set; }
    public string MedicationName { get; private set; } = string.Empty;
    public string Dosage { get; private set; } = string.Empty;
    public string Frequency { get; private set; } = string.Empty;
    public string Duration { get; private set; } = string.Empty;

    public static MedicationItem Create(
        Guid prescriptionId,
        string medicationName,
        string dosage,
        string frequency,
        string duration)
    {
        if (string.IsNullOrWhiteSpace(medicationName))
            throw new ArgumentException("Medication name is required.", nameof(medicationName));
        if (string.IsNullOrWhiteSpace(dosage))
            throw new ArgumentException("Dosage is required.", nameof(dosage));
        if (string.IsNullOrWhiteSpace(frequency))
            throw new ArgumentException("Frequency is required.", nameof(frequency));
        if (string.IsNullOrWhiteSpace(duration))
            throw new ArgumentException("Duration is required.", nameof(duration));

        return new MedicationItem
        {
            Id = Guid.NewGuid(),
            PrescriptionId = prescriptionId,
            MedicationName = medicationName.Trim(),
            Dosage = dosage.Trim(),
            Frequency = frequency.Trim(),
            Duration = duration.Trim()
        };
    }
}
