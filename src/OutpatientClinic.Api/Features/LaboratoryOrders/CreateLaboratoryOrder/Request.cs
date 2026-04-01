namespace OutpatientClinic.Api.Features.LaboratoryOrders.CreateLaboratoryOrder;

public record CreateLaboratoryOrderRequest(
    Guid VisitId,
    List<LaboratoryTestRequest> Tests);

public record LaboratoryTestRequest(
    string TestName,
    string? Notes);
