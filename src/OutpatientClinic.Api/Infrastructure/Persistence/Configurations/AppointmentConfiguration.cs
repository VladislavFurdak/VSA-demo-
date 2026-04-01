using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Appointments;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PatientId).IsRequired();
        builder.Property(x => x.DoctorId).IsRequired();
        builder.Property(x => x.SlotId).IsRequired();
        builder.Property(x => x.ScheduledAtUtc).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.CancellationReason).HasMaxLength(500);

        builder.Ignore(x => x.IsActive);

        builder.HasIndex(x => x.SlotId);
        builder.HasIndex(x => x.PatientId);
    }
}
