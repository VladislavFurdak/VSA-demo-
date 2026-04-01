namespace OutpatientClinic.Api.Domain.Patients;

public class ContactInfo
{
    private ContactInfo() { }

    public string? Phone { get; private set; }
    public string? Email { get; private set; }

    public static ContactInfo Create(string? phone, string? email)
    {
        if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("At least one contact method (phone or email) must be provided.");

        return new ContactInfo
        {
            Phone = phone?.Trim(),
            Email = email?.Trim()
        };
    }

    public ContactInfo Update(string? phone, string? email)
    {
        return Create(phone, email);
    }
}
