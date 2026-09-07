namespace LoanLedger.Application.Loans;
using LoanLedger.Domain.Enums;

public class LoanStatementResponse
{
    public Guid LoanId { get; set; }

	public int Direction { get; set; }

	public string DirectionName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string LoanCategory { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal TotalRepaid { get; set; }

    public decimal TotalInterest { get; set; }

    public decimal TotalPenalty { get; set; }

    public int TransactionCount { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<LoanStatementTransaction> Transactions { get; set; }
        = new();
}