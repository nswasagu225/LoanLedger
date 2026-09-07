namespace LoanLedger.Application.Loans;

using LoanLedger.Domain.Enums;

public class CreateLoanRequest
{
    public Guid? WorkspaceId { get; set; }

    public Guid ContactId { get; set; }

    public Guid LoanCategoryId { get; set; }

    public LoanDirection Direction { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal? InterestRate { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? GuarantorId { get; set; }

    public string? RepaymentPlan { get; set; }

    public string? Notes { get; set; }

    // =========================================================
    // LOAN ITEMS
    // =========================================================

    public List<CreateLoanItemRequest> Items { get; set; }
        = new();
}