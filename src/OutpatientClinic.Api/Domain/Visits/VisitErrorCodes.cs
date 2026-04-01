namespace OutpatientClinic.Api.Domain.Visits;

public static class VisitErrorCodes
{
    public const string NotFound = "visits.not_found";
    public const string AlreadyCompleted = "visits.already_completed";
    public const string MissingMedicalOutcome = "visits.missing_medical_outcome";
    public const string PrescriptionEmpty = "visits.prescription_empty";
    public const string LabOrderEmpty = "visits.lab_order_empty";
    public const string LabOrderNotFound = "visits.lab_order_not_found";
    public const string LabOrderInvalidTransition = "visits.lab_order_invalid_transition";
}
