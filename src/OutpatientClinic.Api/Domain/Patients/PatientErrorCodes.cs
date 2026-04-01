namespace OutpatientClinic.Api.Domain.Patients;

public static class PatientErrorCodes
{
    public const string NotFound = "patients.not_found";
    public const string AlreadyDeactivated = "patients.already_deactivated";
    public const string Deactivated = "patients.deactivated";
}
