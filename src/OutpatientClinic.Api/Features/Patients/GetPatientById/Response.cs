namespace OutpatientClinic.Api.Features.Patients.GetPatientById;

public record GetPatientByIdResponse(
    Guid Id,
    string FullName,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string? Phone,
    string? Email,
    bool IsActive,
    string PaymentBasis,
    InsurancePolicyDto? InsurancePolicy);

public record InsurancePolicyDto(
    string PolicyNumber,
    string Provider,
    DateTime ValidFrom,
    DateTime ValidTo,
    string Status);
