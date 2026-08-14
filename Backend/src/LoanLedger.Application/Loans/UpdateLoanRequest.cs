namespace LoanLedger.Application.Loans;

public class UpdateLoanRequest
{
    public Guid ContactId { get; set; }

    public Guid LoanCategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal InterestRate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? Notes { get; set; }
}