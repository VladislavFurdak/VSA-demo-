using FluentValidation;

namespace OutpatientClinic.Api.Features.Patients.AssignInsurancePolicy;

public class AssignInsurancePolicyRequestValidator : AbstractValidator<AssignInsurancePolicyRequest>
{
    public AssignInsurancePolicyRequestValidator()
    {
        RuleFor(x => x.PolicyNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Provider).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ValidFrom).NotEmpty();
        RuleFor(x => x.ValidTo).NotEmpty().GreaterThan(x => x.ValidFrom);
    }
}
