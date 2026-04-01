using FluentValidation;

namespace OutpatientClinic.Api.Features.Doctors.AddAvailabilitySlot;

public class AddAvailabilitySlotRequestValidator : AbstractValidator<AddAvailabilitySlotRequest>
{
    public AddAvailabilitySlotRequestValidator()
    {
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty().GreaterThan(x => x.StartTime);
    }
}
