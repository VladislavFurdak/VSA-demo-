using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CreateLaboratoryOrder;

public interface ICreateLaboratoryOrderHandler
{
    Task<Result<CreateLaboratoryOrderResponse>> HandleAsync(
        CreateLaboratoryOrderRequest request,
        CancellationToken cancellationToken);
}

public class CreateLaboratoryOrderHandler(AppDbContext dbContext) : ICreateLaboratoryOrderHandler
{
    public async Task<Result<CreateLaboratoryOrderResponse>> HandleAsync(
        CreateLaboratoryOrderRequest request,
        CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == request.VisitId, cancellationToken);

        if (visit is null)
            return Result.Failure<CreateLaboratoryOrderResponse>(VisitErrors.NotFound(request.VisitId));

        if (visit.IsCompleted)
            return Result.Failure<CreateLaboratoryOrderResponse>(VisitErrors.AlreadyCompleted(request.VisitId));

        var tests = request.Tests
            .Select(t => (t.TestName, t.Notes))
            .ToList();

        var order = LaboratoryOrder.Create(request.VisitId, tests);

        dbContext.LaboratoryOrders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateLaboratoryOrderResponse(
            order.Id,
            order.VisitId,
            order.Status.ToString(),
            order.CreatedAtUtc,
            order.TestItems.Select(t => new LaboratoryTestResponse(
                t.Id,
                t.TestName,
                t.Notes)).ToList()));
    }
}
