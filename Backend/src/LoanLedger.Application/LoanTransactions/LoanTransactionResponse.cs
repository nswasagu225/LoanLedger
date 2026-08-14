namespace LoanLedger.Application.LoanTransactions;

public class LoanTransactionResponse
{
    public Guid Id { get; set; }

    public Guid LoanId { get; set; }

    public int TransactionType { get; set; }

    public string TransactionTypeName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public int PaymentMethod { get; set; }

    public string PaymentMethodName { get; set; } = string.Empty;
	
	public int? AdjustmentDirection { get; set; }

	public string? AdjustmentDirectionName { get; set; }

	public int? WaiverType { get; set; }

	public string? WaiverTypeName { get; set; }

    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}