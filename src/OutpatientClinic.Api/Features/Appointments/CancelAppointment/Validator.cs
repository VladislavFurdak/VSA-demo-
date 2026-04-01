using FluentValidation;

namespace OutpatientClinic.Api.Features.Appointments.CancelAppointment;

public class CancelAppointmentRequestValidator : AbstractValidator<CancelAppointmentRequest>
{
    public CancelAppointmentRequestValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.CancelledBy).NotEmpty().Must(x => x is "Patient" or "Clinic")
            .WithMessage("CancelledBy must be 'Patient' or 'Clinic'.");
    }
}
