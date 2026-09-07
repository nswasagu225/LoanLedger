using LoanLedger.Domain.Enums;

namespace LoanLedger.Domain.Services;

public static class LoanBalanceCalculator
{
    public static decimal GetBalanceEffect(
        LoanTransactionType transactionType,
        decimal amount,
        AdjustmentDirection? adjustmentDirection,
        WaiverType? waiverType)
    {
        return transactionType switch
        {
            LoanTransactionType.Disbursement
                => 0,

            LoanTransactionType.Repayment
                => -amount,

            LoanTransactionType.Interest
                => amount,

            LoanTransactionType.Penalty
                => amount,

            LoanTransactionType.Adjustment
                => adjustmentDirection switch
                {
                    AdjustmentDirection.Increase
                        => amount,

                    AdjustmentDirection.Decrease
                        => -amount,

                    _ => throw new Exception(
                        "Adjustment direction is required.")
                },

            LoanTransactionType.Refund
                => amount,

            LoanTransactionType.Waiver
                => waiverType switch
                {
                    WaiverType.Principal => -amount,
                    WaiverType.Interest => -amount,
                    WaiverType.Penalty => -amount,
                    WaiverType.Other => -amount,

                    _ => throw new Exception(
                        "Waiver type is required.")
                },

            _ => throw new Exception(
                "Invalid transaction type.")
        };
    }
}