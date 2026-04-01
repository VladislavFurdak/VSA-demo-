using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Domain.Visits;

public static class VisitErrors
{
    public static Error NotFound(Guid visitId) =>
        Error.NotFound(
            code: VisitErrorCodes.NotFound,
            message: $"Visit '{visitId}' was not found.");

    public static Error AlreadyCompleted(Guid visitId) =>
        Error.BusinessRule(
            code: VisitErrorCodes.AlreadyCompleted,
            message: $"Visit '{visitId}' is already completed and cannot be modified.");

    public static Error MissingMedicalOutcome(Guid visitId) =>
        Error.BusinessRule(
            code: VisitErrorCodes.MissingMedicalOutcome,
            message: $"Visit '{visitId}' must have a documented medical outcome before completion.");

    public static Error PrescriptionEmpty() =>
        Error.BusinessRule(
            code: VisitErrorCodes.PrescriptionEmpty,
            message: "A prescription must contain at least one medication.");

    public static Error LabOrderEmpty() =>
        Error.BusinessRule(
            code: VisitErrorCodes.LabOrderEmpty,
            message: "A laboratory order must contain at least one test.");

    public static Error LabOrderNotFound(Guid orderId) =>
        Error.NotFound(
            code: VisitErrorCodes.LabOrderNotFound,
            message: $"Laboratory order '{orderId}' was not found.");

    public static Error LabOrderInvalidTransition(Guid orderId, string currentStatus) =>
        Error.BusinessRule(
            code: VisitErrorCodes.LabOrderInvalidTransition,
            message: $"Laboratory order '{orderId}' cannot be transitioned from status '{currentStatus}'.");
}
