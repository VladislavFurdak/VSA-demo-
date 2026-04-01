using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Doctors;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class AvailabilitySlotConfiguration : IEntityTypeConfiguration<AvailabilitySlot>
{
    public void Configure(EntityTypeBuilder<AvailabilitySlot> builder)
    {
        builder.ToTable("availability_slots");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DoctorId).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.StartTime).IsRequired();
        builder.Property(x => x.EndTime).IsRequired();
        builder.Property(x => x.IsBlocked).IsRequired();
        builder.Property(x => x.IsBooked).IsRequired();

        builder.Ignore(x => x.IsAvailable);

        builder.HasIndex(x => new { x.DoctorId, x.Date });
    }
}
