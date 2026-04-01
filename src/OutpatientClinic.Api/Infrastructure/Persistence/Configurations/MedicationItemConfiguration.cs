using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Visits;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class MedicationItemConfiguration : IEntityTypeConfiguration<MedicationItem>
{
    public void Configure(EntityTypeBuilder<MedicationItem> builder)
    {
        builder.ToTable("medication_items");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PrescriptionId).IsRequired();
        builder.Property(x => x.MedicationName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Dosage).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Frequency).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Duration).HasMaxLength(100).IsRequired();
    }
}
