namespace LoanLedger.Application.Loans;

public class LoanStatementTransaction
{
    public Guid Id { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public string PaymentMethod { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}