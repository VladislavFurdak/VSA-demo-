namespace OutpatientClinic.Api.Features.Doctors.CreateDoctor;

public record CreateDoctorRequest(
    string FirstName,
    string LastName,
    string Specialization);
