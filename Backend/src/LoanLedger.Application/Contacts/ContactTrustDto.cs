namespace LoanLedger.Application.Contacts;

public class ContactTrustDto
{
    public Guid ContactId { get; set; }

    public Guid WorkspaceId { get; set; }

    public decimal Score { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; }
}