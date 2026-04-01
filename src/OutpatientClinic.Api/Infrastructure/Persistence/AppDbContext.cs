using Microsoft.EntityFrameworkCore;
using OutpatientClinic.Api.Domain.Appointments;
using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Domain.Patients;
using OutpatientClinic.Api.Domain.Visits;

namespace OutpatientClinic.Api.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<AvailabilitySlot> AvailabilitySlots => Set<AvailabilitySlot>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<LaboratoryOrder> LaboratoryOrders => Set<LaboratoryOrder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
