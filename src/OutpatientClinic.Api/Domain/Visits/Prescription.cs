namespace OutpatientClinic.Api.Domain.Visits;

public class Prescription
{
    private readonly List<MedicationItem> _medications = [];

    private Prescription() { }

    public Guid Id { get; private set; }
    public Guid VisitId { get; private set; }
    public DateTime IssuedAtUtc { get; private set; }
    public IReadOnlyList<MedicationItem> Medications => _medications.AsReadOnly();

    public static Prescription Create(Guid visitId, List<(string Name, string Dosage, string Frequency, string Duration)> medications)
    {
        if (medications.Count == 0)
            throw new InvalidOperationException("A prescription must contain at least one medication.");

        var prescription = new Prescription
        {
            Id = Guid.NewGuid(),
            VisitId = visitId,
            IssuedAtUtc = DateTime.UtcNow
        };

        foreach (var med in medications)
        {
            var item = MedicationItem.Create(prescription.Id, med.Name, med.Dosage, med.Frequency, med.Duration);
            prescription._medications.Add(item);
        }

        return prescription;
    }
}
