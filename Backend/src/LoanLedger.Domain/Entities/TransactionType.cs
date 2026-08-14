namespace LoanLedger.Domain.Entities;

public enum TransactionType
{
    Disbursement = 1,

    Repayment = 2,

    Interest = 3,

    Penalty = 4,

    Adjustment = 5,

    Refund = 6,

    Waiver = 7
}