namespace OutpatientClinic.Api.Features.Patients.CreatePatient;

public record CreatePatientResponse(
    Guid Id,
    string FullName,
    DateTime DateOfBirth,
    string? Phone,
    string? Email,
    string PaymentBasis);
