namespace OutpatientClinic.Api.Features.Visits.RecordVisitDetails;

public record RecordVisitDetailsRequest(
    string? Complaints,
    string? Diagnosis,
    string? Recommendations);
