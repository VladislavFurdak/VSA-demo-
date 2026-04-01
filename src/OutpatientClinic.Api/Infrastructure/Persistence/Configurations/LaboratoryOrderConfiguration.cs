using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Visits;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class LaboratoryOrderConfiguration : IEntityTypeConfiguration<LaboratoryOrder>
{
    public void Configure(EntityTypeBuilder<LaboratoryOrder> builder)
    {
        builder.ToTable("laboratory_orders");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.VisitId).IsRequired();
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.HasMany(x => x.TestItems)
            .WithOne()
            .HasForeignKey(x => x.LaboratoryOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
