using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Doctors.GetAvailableSlots;

public interface IGetAvailableSlotsHandler
{
    Task<Result<GetAvailableSlotsResponse>> HandleAsync(
        Guid doctorId,
        DateTime? date,
        CancellationToken cancellationToken);
}

public class GetAvailableSlotsHandler(AppDbContext dbContext) : IGetAvailableSlotsHandler
{
    public async Task<Result<GetAvailableSlotsResponse>> HandleAsync(
        Guid doctorId,
        DateTime? date,
        CancellationToken cancellationToken)
    {
        var doctorExists = await dbContext.Doctors
            .AnyAsync(d => d.Id == doctorId, cancellationToken);

        if (!doctorExists)
            return Result.Failure<GetAvailableSlotsResponse>(DoctorErrors.NotFound(doctorId));

        var query = dbContext.AvailabilitySlots
            .AsNoTracking()
            .Where(s => s.DoctorId == doctorId)
            .Where(s => !s.IsBlocked)
            .Where(s => !s.IsBooked);

        if (date.HasValue)
        {
            query = query.Where(s => s.Date == date.Value.Date);
        }
        else
        {
            query = query.Where(s => s.Date >= DateTime.UtcNow.Date);
        }

        var slots = await query
            .OrderBy(s => s.Date)
            .ThenBy(s => s.StartTime)
            .Select(s => new AvailableSlotDto(s.Id, s.Date, s.StartTime, s.EndTime))
            .ToListAsync(cancellationToken);

        return Result.Success(new GetAvailableSlotsResponse(slots));
    }
}
