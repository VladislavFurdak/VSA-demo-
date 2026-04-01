namespace OutpatientClinic.Api.Features.Visits.OpenVisit;

public record OpenVisitResponse(
    Guid VisitId,
    Guid AppointmentId,
    Guid PatientId,
    Guid DoctorId,
    string Status,
    DateTime OpenedAtUtc);
