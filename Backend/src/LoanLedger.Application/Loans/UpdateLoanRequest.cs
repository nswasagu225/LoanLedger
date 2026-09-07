namespace LoanLedger.Application.Loans;

public class UpdateLoanRequest
{
    public Guid ContactId { get; set; }

    public Guid LoanCategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal? InterestRate { get; set; }

    public DateTime? LoanDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? RepaymentPlan { get; set; }

    public string? Notes { get; set; }

    public List<CreateLoanItemRequest> Items { get; set; }
        = new();
}