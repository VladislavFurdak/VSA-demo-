namespace OutpatientClinic.Api.Domain.Visits;

public class Visit
{
    private readonly List<Prescription> _prescriptions = [];
    private readonly List<LaboratoryOrder> _laboratoryOrders = [];

    private Visit() { }

    public Guid Id { get; private set; }
    public Guid AppointmentId { get; private set; }
    public Guid PatientId { get; private set; }
    public Guid DoctorId { get; private set; }
    public string? Complaints { get; private set; }
    public string? Diagnosis { get; private set; }
    public string? Recommendations { get; private set; }
    public VisitStatus Status { get; private set; }
    public DateTime OpenedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public IReadOnlyList<Prescription> Prescriptions => _prescriptions.AsReadOnly();
    public IReadOnlyList<LaboratoryOrder> LaboratoryOrders => _laboratoryOrders.AsReadOnly();

    public bool IsCompleted => Status == VisitStatus.Completed;

    public static Visit Open(Guid appointmentId, Guid patientId, Guid doctorId)
    {
        return new Visit
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointmentId,
            PatientId = patientId,
            DoctorId = doctorId,
            Status = VisitStatus.Open,
            OpenedAtUtc = DateTime.UtcNow
        };
    }

    public void RecordDetails(string? complaints, string? diagnosis, string? recommendations)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot modify a completed visit.");

        Complaints = complaints;
        Diagnosis = diagnosis;
        Recommendations = recommendations;
    }

    public void Complete()
    {
        if (IsCompleted)
            throw new InvalidOperationException("Visit is already completed.");

        if (string.IsNullOrWhiteSpace(Diagnosis) && string.IsNullOrWhiteSpace(Recommendations))
            throw new InvalidOperationException("A visit must have a documented medical outcome (diagnosis or recommendations) before completion.");

        Status = VisitStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public Prescription AddPrescription(List<(string Name, string Dosage, string Frequency, string Duration)> medications)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot add a prescription to a completed visit.");

        var prescription = Prescription.Create(Id, medications);
        _prescriptions.Add(prescription);
        return prescription;
    }

    public LaboratoryOrder AddLaboratoryOrder(List<(string TestName, string? Notes)> tests)
    {
        if (IsCompleted)
            throw new InvalidOperationException("Cannot add a laboratory order to a completed visit.");

        var order = LaboratoryOrder.Create(Id, tests);
        _laboratoryOrders.Add(order);
        return order;
    }
}
