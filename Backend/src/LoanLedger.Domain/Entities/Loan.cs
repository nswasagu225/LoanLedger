namespace LoanLedger.Domain.Entities;

public class Loan
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    public Guid LoanCategoryId { get; set; }

	public LoanCategory LoanCategory { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string Notes { get; set; } = string.Empty;

    public bool IsClosed { get; set; }

    public DateTime? ClosedAt { get; set; }
	public ICollection<LoanTransaction> Transactions { get; set; }
    = new List<LoanTransaction>();
}