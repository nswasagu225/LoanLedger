namespace LoanLedger.Application.Items;

public class CreateItemResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid? ItemId { get; set; }
}