using FluentValidation;

namespace OutpatientClinic.Api.Features.Visits.RecordVisitDetails;

public class RecordVisitDetailsRequestValidator : AbstractValidator<RecordVisitDetailsRequest>
{
    public RecordVisitDetailsRequestValidator()
    {
        RuleFor(x => x.Complaints).MaximumLength(2000);
        RuleFor(x => x.Diagnosis).MaximumLength(2000);
        RuleFor(x => x.Recommendations).MaximumLength(2000);
    }
}
