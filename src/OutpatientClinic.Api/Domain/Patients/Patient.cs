namespace OutpatientClinic.Api.Domain.Patients;

public class Patient
{
    private Patient() { }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string? MiddleName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public ContactInfo Contact { get; private set; } = null!;
    public InsurancePolicy? InsurancePolicy { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    public string FullName => string.IsNullOrWhiteSpace(MiddleName)
        ? $"{LastName} {FirstName}"
        : $"{LastName} {FirstName} {MiddleName}";

    public static Patient Create(
        string firstName,
        string lastName,
        string? middleName,
        DateTime dateOfBirth,
        string? phone,
        string? email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        return new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            MiddleName = middleName?.Trim(),
            DateOfBirth = dateOfBirth,
            Contact = ContactInfo.Create(phone, email),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void UpdateContacts(string? phone, string? email)
    {
        Contact = ContactInfo.Create(phone, email);
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void AssignInsurancePolicy(InsurancePolicy policy)
    {
        InsurancePolicy = policy;
    }

    public PaymentBasis DeterminePaymentBasis(DateTime utcNow)
    {
        if (InsurancePolicy is null || !InsurancePolicy.IsCurrentlyValid(utcNow))
            return PaymentBasis.SelfPaid;

        return PaymentBasis.Insurance;
    }
}
