using FluentValidation;

namespace OutpatientClinic.Api.Features.Visits.OpenVisit;

public class OpenVisitRequestValidator : AbstractValidator<OpenVisitRequest>
{
    public OpenVisitRequestValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty();
    }
}
