namespace LoanLedger.Application.Loans;

public class CreateLoanItemRequest
{
    // Optional: selected from the user's Item catalog
    public Guid? ItemId { get; set; }

    // Required when using a catalog item or entering a custom item
    public string ItemName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    public decimal Quantity { get; set; }

    public decimal Rate { get; set; }

    public string? Notes { get; set; }
}