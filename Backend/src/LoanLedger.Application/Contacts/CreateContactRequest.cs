using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Contacts;

public class CreateContactRequest
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? BusinessName { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public ContactType ContactType { get; set; }
}