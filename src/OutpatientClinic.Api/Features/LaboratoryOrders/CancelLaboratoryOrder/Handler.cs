using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CancelLaboratoryOrder;

public interface ICancelLaboratoryOrderHandler
{
    Task<Result> HandleAsync(Guid orderId, CancellationToken cancellationToken);
}

public class CancelLaboratoryOrderHandler(AppDbContext dbContext) : ICancelLaboratoryOrderHandler
{
    public async Task<Result> HandleAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await dbContext.LaboratoryOrders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
            return Result.Failure(VisitErrors.LabOrderNotFound(orderId));

        if (order.Status is LaboratoryOrderStatus.Completed or LaboratoryOrderStatus.Cancelled)
            return Result.Failure(VisitErrors.LabOrderInvalidTransition(orderId, order.Status.ToString()));

        order.Cancel();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
