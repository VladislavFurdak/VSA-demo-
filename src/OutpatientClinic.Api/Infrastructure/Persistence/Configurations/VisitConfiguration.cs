using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Visits;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("visits");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AppointmentId).IsRequired();
        builder.Property(x => x.PatientId).IsRequired();
        builder.Property(x => x.DoctorId).IsRequired();
        builder.Property(x => x.Complaints).HasMaxLength(2000);
        builder.Property(x => x.Diagnosis).HasMaxLength(2000);
        builder.Property(x => x.Recommendations).HasMaxLength(2000);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.OpenedAtUtc).IsRequired();

        builder.Ignore(x => x.IsCompleted);

        builder.HasIndex(x => x.AppointmentId).IsUnique();

        builder.HasMany(x => x.Prescriptions)
            .WithOne()
            .HasForeignKey(x => x.VisitId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.LaboratoryOrders)
            .WithOne()
            .HasForeignKey(x => x.VisitId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
