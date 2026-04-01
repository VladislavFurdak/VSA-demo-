using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using ServiceDefaults;
using OutpatientClinic.Api.Infrastructure.Persistence;

// Features - Patients
using OutpatientClinic.Api.Features.Patients.CreatePatient;
using OutpatientClinic.Api.Features.Patients.UpdatePatientContacts;
using OutpatientClinic.Api.Features.Patients.DeactivatePatient;
using OutpatientClinic.Api.Features.Patients.AssignInsurancePolicy;
using OutpatientClinic.Api.Features.Patients.GetPatientById;

// Features - Doctors
using OutpatientClinic.Api.Features.Doctors.CreateDoctor;
using OutpatientClinic.Api.Features.Doctors.AddAvailabilitySlot;
using OutpatientClinic.Api.Features.Doctors.BlockAvailabilitySlot;
using OutpatientClinic.Api.Features.Doctors.GetAvailableSlots;

// Features - Appointments
using OutpatientClinic.Api.Features.Appointments.BookAppointment;
using OutpatientClinic.Api.Features.Appointments.ConfirmAppointment;
using OutpatientClinic.Api.Features.Appointments.CancelAppointment;
using OutpatientClinic.Api.Features.Appointments.RescheduleAppointment;
using OutpatientClinic.Api.Features.Appointments.MarkNoShow;

// Features - Visits
using OutpatientClinic.Api.Features.Visits.OpenVisit;
using OutpatientClinic.Api.Features.Visits.RecordVisitDetails;
using OutpatientClinic.Api.Features.Visits.CompleteVisit;

// Features - Prescriptions
using OutpatientClinic.Api.Features.Prescriptions.CreatePrescription;

// Features - Laboratory Orders
using OutpatientClinic.Api.Features.LaboratoryOrders.CreateLaboratoryOrder;
using OutpatientClinic.Api.Features.LaboratoryOrders.CompleteLaboratoryOrder;
using OutpatientClinic.Api.Features.LaboratoryOrders.CancelLaboratoryOrder;

var builder = WebApplication.CreateBuilder(args);

// Aspire ServiceDefaults: OpenTelemetry, health checks, resilience, service discovery
builder.AddServiceDefaults();

builder.Host.UseSerilog((context, services, logger) =>
{
    logger
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

builder.Services.AddOpenApi();

// EF Core + Npgsql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("clinic-db")));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreatePatientRequestValidator>();

// Handlers - Patients
builder.Services.AddScoped<ICreatePatientHandler, CreatePatientHandler>();
builder.Services.AddScoped<IUpdatePatientContactsHandler, UpdatePatientContactsHandler>();
builder.Services.AddScoped<IDeactivatePatientHandler, DeactivatePatientHandler>();
builder.Services.AddScoped<IAssignInsurancePolicyHandler, AssignInsurancePolicyHandler>();
builder.Services.AddScoped<IGetPatientByIdHandler, GetPatientByIdHandler>();

// Handlers - Doctors
builder.Services.AddScoped<ICreateDoctorHandler, CreateDoctorHandler>();
builder.Services.AddScoped<IAddAvailabilitySlotHandler, AddAvailabilitySlotHandler>();
builder.Services.AddScoped<IBlockAvailabilitySlotHandler, BlockAvailabilitySlotHandler>();
builder.Services.AddScoped<IGetAvailableSlotsHandler, GetAvailableSlotsHandler>();

// Handlers - Appointments
builder.Services.AddScoped<IBookAppointmentHandler, BookAppointmentHandler>();
builder.Services.AddScoped<IConfirmAppointmentHandler, ConfirmAppointmentHandler>();
builder.Services.AddScoped<ICancelAppointmentHandler, CancelAppointmentHandler>();
builder.Services.AddScoped<IRescheduleAppointmentHandler, RescheduleAppointmentHandler>();
builder.Services.AddScoped<IMarkNoShowHandler, MarkNoShowHandler>();

// Handlers - Visits
builder.Services.AddScoped<IOpenVisitHandler, OpenVisitHandler>();
builder.Services.AddScoped<IRecordVisitDetailsHandler, RecordVisitDetailsHandler>();
builder.Services.AddScoped<ICompleteVisitHandler, CompleteVisitHandler>();

// Handlers - Prescriptions
builder.Services.AddScoped<ICreatePrescriptionHandler, CreatePrescriptionHandler>();

// Handlers - Laboratory Orders
builder.Services.AddScoped<ICreateLaboratoryOrderHandler, CreateLaboratoryOrderHandler>();
builder.Services.AddScoped<ICompleteLaboratoryOrderHandler, CompleteLaboratoryOrderHandler>();
builder.Services.AddScoped<ICancelLaboratoryOrderHandler, CancelLaboratoryOrderHandler>();

// Health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>("AppDbContext-ready", tags: ["ready"]);

builder.Services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromSeconds(30);
});

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandler("/error");
app.Map("/error", () => Results.Problem(
    title: "An unexpected error occurred.",
    statusCode: StatusCodes.Status500InternalServerError));

// ServiceDefaults: maps /health/live and /health/ready endpoints
app.MapDefaultEndpoints();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.Title = "Outpatient Clinic API";
    options.Theme = ScalarTheme.DeepSpace;
    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/scalar/v1"))
        .ExcludeFromDescription();
}

// Patients
app.MapCreatePatient();
app.MapUpdatePatientContacts();
app.MapDeactivatePatient();
app.MapAssignInsurancePolicy();
app.MapGetPatientById();

// Doctors
app.MapCreateDoctor();
app.MapAddAvailabilitySlot();
app.MapBlockAvailabilitySlot();
app.MapGetAvailableSlots();

// Appointments
app.MapBookAppointment();
app.MapConfirmAppointment();
app.MapCancelAppointment();
app.MapRescheduleAppointment();
app.MapMarkNoShow();

// Visits
app.MapOpenVisit();
app.MapRecordVisitDetails();
app.MapCompleteVisit();

// Prescriptions
app.MapCreatePrescription();

// Laboratory Orders
app.MapCreateLaboratoryOrder();
app.MapCompleteLaboratoryOrder();
app.MapCancelLaboratoryOrder();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() =>
    Log.Information("Outpatient Clinic API started"));

lifetime.ApplicationStopping.Register(() =>
    Log.Information("Outpatient Clinic API is shutting down..."));

lifetime.ApplicationStopped.Register(() =>
{
    Log.Information("Outpatient Clinic API stopped");
    Log.CloseAndFlush();
});

app.Run();
