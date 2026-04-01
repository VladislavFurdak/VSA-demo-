using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Domain.Patients;

public static class PatientErrors
{
    public static Error NotFound(Guid patientId) =>
        Error.NotFound(
            code: PatientErrorCodes.NotFound,
            message: $"Patient '{patientId}' was not found.");

    public static Error AlreadyDeactivated(Guid patientId) =>
        Error.Conflict(
            code: PatientErrorCodes.AlreadyDeactivated,
            message: $"Patient '{patientId}' is already deactivated.");

    public static Error Deactivated(Guid patientId) =>
        Error.BusinessRule(
            code: PatientErrorCodes.Deactivated,
            message: $"Patient '{patientId}' is deactivated and cannot be booked.");
}
