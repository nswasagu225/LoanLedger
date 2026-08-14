using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.LoanTransactions;

public class CreateLoanTransactionRequest
{
    public Guid LoanId { get; set; }

    public LoanTransactionType TransactionType { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string ReferenceNumber { get; set; } = string.Empty;

    public PaymentMethod PaymentMethod { get; set; }

    public string Description { get; set; } = string.Empty;

    public AdjustmentDirection? AdjustmentDirection { get; set; }

    public WaiverType? WaiverType { get; set; }
}