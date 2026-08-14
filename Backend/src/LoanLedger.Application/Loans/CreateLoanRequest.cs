namespace LoanLedger.Application.Loans;

public class CreateLoanRequest
{
    public Guid UserId { get; set; }

    public Guid ContactId { get; set; }

    public Guid LoanCategoryId { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string Notes { get; set; } = string.Empty;
}