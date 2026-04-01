namespace OutpatientClinic.Api.Features.Patients.AssignInsurancePolicy;

public record AssignInsurancePolicyResponse(
    Guid PatientId,
    string PolicyNumber,
    string Provider,
    string Status,
    string PaymentBasis);
