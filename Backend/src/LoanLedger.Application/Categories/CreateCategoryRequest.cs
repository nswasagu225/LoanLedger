namespace LoanLedger.Application.Categories;

public class CreateCategoryRequest
{
    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Color { get; set; } = "#4CAF50";

    public string Icon { get; set; } = "category";
}