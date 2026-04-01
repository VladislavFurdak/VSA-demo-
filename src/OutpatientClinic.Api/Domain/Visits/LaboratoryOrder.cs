namespace OutpatientClinic.Api.Domain.Visits;

public class LaboratoryOrder
{
    private readonly List<LaboratoryTestItem> _testItems = [];

    private LaboratoryOrder() { }

    public Guid Id { get; private set; }
    public Guid VisitId { get; private set; }
    public LaboratoryOrderStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? CompletedAtUtc { get; private set; }
    public IReadOnlyList<LaboratoryTestItem> TestItems => _testItems.AsReadOnly();

    public static LaboratoryOrder Create(Guid visitId, List<(string TestName, string? Notes)> tests)
    {
        if (tests.Count == 0)
            throw new InvalidOperationException("A laboratory order must contain at least one test.");

        var order = new LaboratoryOrder
        {
            Id = Guid.NewGuid(),
            VisitId = visitId,
            Status = LaboratoryOrderStatus.Created,
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (var test in tests)
        {
            var item = LaboratoryTestItem.Create(order.Id, test.TestName, test.Notes);
            order._testItems.Add(item);
        }

        return order;
    }

    public void SendToLab()
    {
        if (Status != LaboratoryOrderStatus.Created)
            throw new InvalidOperationException($"Cannot send order in status '{Status}' to lab.");
        Status = LaboratoryOrderStatus.SentToLab;
    }

    public void Complete()
    {
        if (Status is not (LaboratoryOrderStatus.Created or LaboratoryOrderStatus.SentToLab))
            throw new InvalidOperationException($"Cannot complete order in status '{Status}'.");
        Status = LaboratoryOrderStatus.Completed;
        CompletedAtUtc = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status is LaboratoryOrderStatus.Completed or LaboratoryOrderStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel order in status '{Status}'.");
        Status = LaboratoryOrderStatus.Cancelled;
    }
}
