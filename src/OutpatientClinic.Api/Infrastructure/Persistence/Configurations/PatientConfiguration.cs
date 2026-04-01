using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OutpatientClinic.Api.Domain.Patients;

namespace OutpatientClinic.Api.Infrastructure.Persistence.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.ToTable("patients");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MiddleName).HasMaxLength(100);
        builder.Property(x => x.DateOfBirth).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Ignore(x => x.FullName);

        builder.OwnsOne(x => x.Contact, contact =>
        {
            contact.Property(c => c.Phone).HasColumnName("Phone").HasMaxLength(50);
            contact.Property(c => c.Email).HasColumnName("Email").HasMaxLength(256);
        });

        builder.OwnsOne(x => x.InsurancePolicy, policy =>
        {
            policy.Property(p => p.PolicyNumber).HasColumnName("InsurancePolicyNumber").HasMaxLength(100);
            policy.Property(p => p.Provider).HasColumnName("InsuranceProvider").HasMaxLength(200);
            policy.Property(p => p.ValidFrom).HasColumnName("InsuranceValidFrom");
            policy.Property(p => p.ValidTo).HasColumnName("InsuranceValidTo");
            policy.Property(p => p.Status).HasColumnName("InsuranceStatus").HasConversion<string>().HasMaxLength(50);
        });
    }
}
