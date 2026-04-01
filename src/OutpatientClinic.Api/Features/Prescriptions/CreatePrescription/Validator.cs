using FluentValidation;

namespace OutpatientClinic.Api.Features.Prescriptions.CreatePrescription;

public class CreatePrescriptionRequestValidator : AbstractValidator<CreatePrescriptionRequest>
{
    public CreatePrescriptionRequestValidator()
    {
        RuleFor(x => x.VisitId).NotEmpty();
        RuleFor(x => x.Medications).NotEmpty()
            .WithMessage("A prescription must contain at least one medication.");

        RuleForEach(x => x.Medications).ChildRules(med =>
        {
            med.RuleFor(m => m.MedicationName).NotEmpty().MaximumLength(200);
            med.RuleFor(m => m.Dosage).NotEmpty().MaximumLength(100);
            med.RuleFor(m => m.Frequency).NotEmpty().MaximumLength(100);
            med.RuleFor(m => m.Duration).NotEmpty().MaximumLength(100);
        });
    }
}
