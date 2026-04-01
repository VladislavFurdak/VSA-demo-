using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Visits;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Visits.RecordVisitDetails;

public interface IRecordVisitDetailsHandler
{
    Task<Result> HandleAsync(Guid visitId, RecordVisitDetailsRequest request, CancellationToken cancellationToken);
}

public class RecordVisitDetailsHandler(AppDbContext dbContext) : IRecordVisitDetailsHandler
{
    public async Task<Result> HandleAsync(
        Guid visitId,
        RecordVisitDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var visit = await dbContext.Visits
            .FirstOrDefaultAsync(v => v.Id == visitId, cancellationToken);

        if (visit is null)
            return Result.Failure(VisitErrors.NotFound(visitId));

        if (visit.IsCompleted)
            return Result.Failure(VisitErrors.AlreadyCompleted(visitId));

        visit.RecordDetails(request.Complaints, request.Diagnosis, request.Recommendations);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
