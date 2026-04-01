var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgres-password", secret: true);

var postgres = builder.AddPostgres("postgres", password: postgresPassword)
    .WithPgAdmin()
    .WithDataVolume(isReadOnly: false)
    .WithLifetime(ContainerLifetime.Persistent);

var clinicDb = postgres.AddDatabase("clinic-db");

builder.AddProject<Projects.OutpatientClinic_Api>("outpatient-clinic-api")
    .WithReference(clinicDb)
    .WaitFor(clinicDb)
    .WithExternalHttpEndpoints();

builder.Build().Run();
