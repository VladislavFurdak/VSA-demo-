using FluentValidation;

namespace OutpatientClinic.Api.Features.Appointments.BookAppointment;

public class BookAppointmentRequestValidator : AbstractValidator<BookAppointmentRequest>
{
    public BookAppointmentRequestValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.DoctorId).NotEmpty();
        RuleFor(x => x.SlotId).NotEmpty();
    }
}
