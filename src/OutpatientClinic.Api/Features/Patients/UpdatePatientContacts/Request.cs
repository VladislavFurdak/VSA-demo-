namespace OutpatientClinic.Api.Features.Patients.UpdatePatientContacts;

public record UpdatePatientContactsRequest(
    string? Phone,
    string? Email);
