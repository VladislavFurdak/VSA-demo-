using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Visits;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class LaboratoryTestItemConfiguration : IEntityTypeConfiguration<LaboratoryTestItem>
{
    public void Configure(EntityTypeBuilder<LaboratoryTestItem> builder)
    {
        builder.ToTable("laboratory_test_items");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.LaboratoryOrderId).IsRequired();
        builder.Property(x => x.TestName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Notes).HasMaxLength(500);
    }
}
