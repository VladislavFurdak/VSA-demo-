using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Visits;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.ToTable("prescriptions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.VisitId).IsRequired();
        builder.Property(x => x.IssuedAtUtc).IsRequired();

        builder.HasMany(x => x.Medications)
            .WithOne()
            .HasForeignKey(x => x.PrescriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
