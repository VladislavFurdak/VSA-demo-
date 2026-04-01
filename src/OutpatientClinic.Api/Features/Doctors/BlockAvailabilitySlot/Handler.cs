using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Doctors.BlockAvailabilitySlot;

public interface IBlockAvailabilitySlotHandler
{
    Task<Result> HandleAsync(Guid doctorId, Guid slotId, CancellationToken cancellationToken);
}

public class BlockAvailabilitySlotHandler(AppDbContext dbContext) : IBlockAvailabilitySlotHandler
{
    public async Task<Result> HandleAsync(Guid doctorId, Guid slotId, CancellationToken cancellationToken)
    {
        var slot = await dbContext.AvailabilitySlots
            .FirstOrDefaultAsync(s => s.Id == slotId && s.DoctorId == doctorId, cancellationToken);

        if (slot is null)
            return Result.Failure(DoctorErrors.SlotNotFound(slotId));

        if (slot.IsBooked)
            return Result.Failure(DoctorErrors.SlotAlreadyBooked(slotId));

        slot.Block();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
