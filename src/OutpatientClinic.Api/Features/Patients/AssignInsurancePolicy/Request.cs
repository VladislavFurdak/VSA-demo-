namespace OutpatientClinic.Api.Features.Patients.AssignInsurancePolicy;

public record AssignInsurancePolicyRequest(
    string PolicyNumber,
    string Provider,
    DateTime ValidFrom,
    DateTime ValidTo);
