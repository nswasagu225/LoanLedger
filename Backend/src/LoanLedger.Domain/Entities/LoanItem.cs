namespace LoanLedger.Domain.Entities;

public class LoanItem
{
    public Guid Id { get; set; }

    // =========================================================
    // LOAN
    // =========================================================

    public Guid LoanId { get; set; }

    public Loan Loan { get; set; } = null!;

    // =========================================================
    // OPTIONAL ITEM CATALOG REFERENCE
    // =========================================================

    public Guid? ItemId { get; set; }

    public Item? Item { get; set; }

    // =========================================================
    // ITEM SNAPSHOT
    // =========================================================

    public string ItemName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    // =========================================================
    // LOAN-SPECIFIC VALUES
    // =========================================================

    public decimal Quantity { get; set; }

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }

    // =========================================================
    // NOTES
    // =========================================================

    public string? Notes { get; set; }
}