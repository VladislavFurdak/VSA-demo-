namespace OutpatientClinic.Api.Features.LaboratoryOrders.CreateLaboratoryOrder;

public record CreateLaboratoryOrderResponse(
    Guid OrderId,
    Guid VisitId,
    string Status,
    DateTime CreatedAtUtc,
    List<LaboratoryTestResponse> Tests);

public record LaboratoryTestResponse(
    Guid Id,
    string TestName,
    string? Notes);
