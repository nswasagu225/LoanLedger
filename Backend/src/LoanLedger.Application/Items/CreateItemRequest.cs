namespace LoanLedger.Application.Items;

public class CreateItemRequest
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Unit { get; set; }

    public decimal DefaultRate { get; set; }
}