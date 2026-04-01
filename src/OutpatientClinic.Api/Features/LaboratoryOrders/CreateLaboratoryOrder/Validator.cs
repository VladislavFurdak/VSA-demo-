using FluentValidation;

namespace OutpatientClinic.Api.Features.LaboratoryOrders.CreateLaboratoryOrder;

public class CreateLaboratoryOrderRequestValidator : AbstractValidator<CreateLaboratoryOrderRequest>
{
    public CreateLaboratoryOrderRequestValidator()
    {
        RuleFor(x => x.VisitId).NotEmpty();
        RuleFor(x => x.Tests).NotEmpty()
            .WithMessage("A laboratory order must contain at least one test.");

        RuleForEach(x => x.Tests).ChildRules(test =>
        {
            test.RuleFor(t => t.TestName).NotEmpty().MaximumLength(200);
            test.RuleFor(t => t.Notes).MaximumLength(500);
        });
    }
}
