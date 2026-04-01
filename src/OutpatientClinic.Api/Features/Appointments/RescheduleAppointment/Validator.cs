using FluentValidation;

namespace OutpatientClinic.Api.Features.Appointments.RescheduleAppointment;

public class RescheduleAppointmentRequestValidator : AbstractValidator<RescheduleAppointmentRequest>
{
    public RescheduleAppointmentRequestValidator()
    {
        RuleFor(x => x.NewSlotId).NotEmpty();
    }
}
