namespace LoanLedger.Application.Categories;

public class CreateCategoryResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid CategoryId { get; set; }
}