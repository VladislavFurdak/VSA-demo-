namespace OutpatientClinic.Api.Domain.Visits;

public class LaboratoryTestItem
{
    private LaboratoryTestItem() { }

    public Guid Id { get; private set; }
    public Guid LaboratoryOrderId { get; private set; }
    public string TestName { get; private set; } = string.Empty;
    public string? Notes { get; private set; }

    public static LaboratoryTestItem Create(Guid laboratoryOrderId, string testName, string? notes)
    {
        if (string.IsNullOrWhiteSpace(testName))
            throw new ArgumentException("Test name is required.", nameof(testName));

        return new LaboratoryTestItem
        {
            Id = Guid.NewGuid(),
            LaboratoryOrderId = laboratoryOrderId,
            TestName = testName.Trim(),
            Notes = notes?.Trim()
        };
    }
}
