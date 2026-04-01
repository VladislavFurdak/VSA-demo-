namespace OutpatientClinic.Api.Features.Patients.CreatePatient;

public record CreatePatientRequest(
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime DateOfBirth,
    string? Phone,
    string? Email);
