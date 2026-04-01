namespace OutpatientClinic.Api.Features.Prescriptions.CreatePrescription;

public record CreatePrescriptionResponse(
    Guid PrescriptionId,
    Guid VisitId,
    DateTime IssuedAtUtc,
    List<MedicationItemResponse> Medications);

public record MedicationItemResponse(
    Guid Id,
    string MedicationName,
    string Dosage,
    string Frequency,
    string Duration);
