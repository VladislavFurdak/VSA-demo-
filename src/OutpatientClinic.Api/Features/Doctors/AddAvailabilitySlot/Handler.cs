using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Doctors.AddAvailabilitySlot;

public interface IAddAvailabilitySlotHandler
{
    Task<Result<AddAvailabilitySlotResponse>> HandleAsync(
        Guid doctorId,
        AddAvailabilitySlotRequest request,
        CancellationToken cancellationToken);
}

public class AddAvailabilitySlotHandler(AppDbContext dbContext)
    : IAddAvailabilitySlotHandler
{
    public async Task<Result<AddAvailabilitySlotResponse>> HandleAsync(
        Guid doctorId,
        AddAvailabilitySlotRequest request,
        CancellationToken cancellationToken)
    {
        var doctor = await dbContext.Doctors
            .Include(d => d.AvailabilitySlots.Where(s => s.Date == request.Date))
            .FirstOrDefaultAsync(d => d.Id == doctorId, cancellationToken);

        if (doctor is null)
            return Result.Failure<AddAvailabilitySlotResponse>(DoctorErrors.NotFound(doctorId));

        var hasOverlap = doctor.AvailabilitySlots.Any(s =>
            s.StartTime < request.EndTime && s.EndTime > request.StartTime);

        if (hasOverlap)
            return Result.Failure<AddAvailabilitySlotResponse>(DoctorErrors.SlotOverlap());

        var slot = AvailabilitySlot.Create(doctorId, request.Date, request.StartTime, request.EndTime);
        dbContext.AvailabilitySlots.Add(slot);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(new AddAvailabilitySlotResponse(
            slot.Id,
            doctorId,
            slot.Date,
            slot.StartTime,
            slot.EndTime));
    }
}
