namespace LoanLedger.Application.Contacts;

public class CreateContactResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid ContactId { get; set; }
}