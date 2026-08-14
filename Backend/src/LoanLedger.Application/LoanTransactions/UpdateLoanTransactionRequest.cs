using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.LoanTransactions;

public class UpdateLoanTransactionRequest
{
    public LoanTransactionType TransactionType { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string? Description { get; set; }

    public AdjustmentDirection? AdjustmentDirection { get; set; }

    public WaiverType? WaiverType { get; set; }
}