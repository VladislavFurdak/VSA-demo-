using OutpatientClinic.Api.Domain.Doctors;
using OutpatientClinic.Api.Infrastructure.Persistence;
using OutpatientClinic.Api.Shared.Results;

namespace OutpatientClinic.Api.Features.Doctors.CreateDoctor;

public interface ICreateDoctorHandler
{
    Task<Result<CreateDoctorResponse>> HandleAsync(CreateDoctorRequest request, CancellationToken cancellationToken);
}

public class CreateDoctorHandler(
    AppDbContext dbContext,
    ILogger<CreateDoctorHandler> logger)
    : ICreateDoctorHandler
{
    public async Task<Result<CreateDoctorResponse>> HandleAsync(
        CreateDoctorRequest request,
        CancellationToken cancellationToken)
    {
        var doctor = Doctor.Create(request.FirstName, request.LastName, request.Specialization);

        dbContext.Doctors.Add(doctor);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Created doctor {DoctorId} ({FullName})", doctor.Id, doctor.FullName);

        return Result.Success(new CreateDoctorResponse(
            doctor.Id,
            doctor.FullName,
            doctor.Specialization));
    }
}
