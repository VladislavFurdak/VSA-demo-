namespace OutpatientClinic.Api.Features.Prescriptions.CreatePrescription;

public record CreatePrescriptionRequest(
    Guid VisitId,
    List<MedicationItemRequest> Medications);

public record MedicationItemRequest(
    string MedicationName,
    string Dosage,
    string Frequency,
    string Duration);
