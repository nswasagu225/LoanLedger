namespace LoanLedger.Application.Loans;

public class LoanItemDto
{
    public Guid Id { get; set; }

    public Guid? ItemId { get; set; }

    public string ItemName { get; set; } = string.Empty;

    public string? Unit { get; set; }

    public decimal Quantity { get; set; }

    public decimal Rate { get; set; }

    public decimal Amount { get; set; }

    public string? Notes { get; set; }
}