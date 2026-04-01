namespace OutpatientClinic.Api.Features.Doctors.CreateDoctor;

public record CreateDoctorResponse(
    Guid Id,
    string FullName,
    string Specialization);
