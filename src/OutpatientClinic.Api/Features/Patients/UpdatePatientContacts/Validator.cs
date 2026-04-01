using FluentValidation;

namespace OutpatientClinic.Api.Features.Patients.UpdatePatientContacts;

public class UpdatePatientContactsRequestValidator : AbstractValidator<UpdatePatientContactsRequest>
{
    public UpdatePatientContactsRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Phone) || !string.IsNullOrWhiteSpace(x.Email))
            .WithMessage("At least one contact method (phone or email) must be provided.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
