using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CompleteLaboratoryOrder;

public interface ICompleteLaboratoryOrderHandler
{
    Task<Result> HandleAsync(Guid orderId, CancellationToken cancellationToken);
}

public class CompleteLaboratoryOrderHandler(AppDbContext dbContext) : ICompleteLaboratoryOrderHandler
{
    public async Task<Result> HandleAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.LaboratoryOrders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Result.Failure(VisitErrors.LabOrderNotFound(orderId));

        if (order.Status is not (LaboratoryOrderStatus.Created or LaboratoryOrderStatus.SentToLab))
            return Result.Failure(VisitErrors.LabOrderInvalidTransition(orderId, order.Status.ToString()));

        order.Complete();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
