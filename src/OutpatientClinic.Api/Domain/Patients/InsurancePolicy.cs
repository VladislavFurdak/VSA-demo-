namespace OutpatientClinic.Api.Domain.Patients;

public class InsurancePolicy
{
    private InsurancePolicy() { }

    public string PolicyNumber { get; private set; } = string.Empty;
    public string Provider { get; private set; } = string.Empty;
    public DateTime ValidFrom { get; private set; }
    public DateTime ValidTo { get; private set; }
    public InsurancePolicyStatus Status { get; private set; }

    public static InsurancePolicy Create(
        string policyNumber,
        string provider,
        DateTime validFrom,
        DateTime validTo)
    {
        if (string.IsNullOrWhiteSpace(policyNumber))
            throw new ArgumentException("Policy number is required.", nameof(policyNumber));
        if (string.IsNullOrWhiteSpace(provider))
            throw new ArgumentException("Provider is required.", nameof(provider));
        if (validTo <= validFrom)
            throw new ArgumentException("ValidTo must be after ValidFrom.");

        return new InsurancePolicy
        {
            PolicyNumber = policyNumber.Trim(),
            Provider = provider.Trim(),
            ValidFrom = validFrom,
            ValidTo = validTo,
            Status = InsurancePolicyStatus.Valid
        };
    }

    public bool IsCurrentlyValid(DateTime utcNow)
    {
        return Status == InsurancePolicyStatus.Valid
               && utcNow >= ValidFrom
               && utcNow <= ValidTo;
    }

    public void MarkExpired() => Status = InsurancePolicyStatus.Expired;

    public void MarkTemporarilyInvalid() => Status = InsurancePolicyStatus.TemporarilyInvalid;

    public void Revalidate() => Status = InsurancePolicyStatus.Valid;
}
