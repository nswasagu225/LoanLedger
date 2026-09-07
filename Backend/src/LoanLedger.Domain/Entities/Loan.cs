namespace LoanLedger.Domain.Entities;
using LoanLedger.Domain.Enums;

public class Loan
{
    public Guid Id { get; set; }

    // =========================================================
    // OWNER
    // =========================================================

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    // =========================================================
    // WORKSPACE
    // =========================================================

    public Guid? WorkspaceId { get; set; }

	public Workspace? Workspace { get; set; }

    // =========================================================
    // CONTACT
    // =========================================================

    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    // =========================================================
    // LOAN CATEGORY
    // =========================================================

    public Guid LoanCategoryId { get; set; }

    public LoanCategory LoanCategory { get; set; } = null!;

    // =========================================================
    // LOAN INFORMATION
    // =========================================================
	
	public LoanDirection Direction { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal PrincipalAmount { get; set; }

    public decimal CurrentBalance { get; set; }

    public decimal InterestRate { get; set; }

    public DateTime LoanDate { get; set; }

    public DateTime? DueDate { get; set; }
	
	public Guid? GuarantorId { get; set; }

	public Guarantor? Guarantor { get; set; }

	public string? RepaymentPlan { get; set; }

    public string? Notes { get; set; }

    public bool IsClosed { get; set; }

    public DateTime? ClosedAt { get; set; }

    // =========================================================
	// TRANSACTIONS
	// =========================================================

	public ICollection<LoanTransaction> Transactions { get; set; }
		= new List<LoanTransaction>();

	// =========================================================
	// LOAN ITEMS
	// =========================================================

	public ICollection<LoanItem> LoanItems { get; set; }
		= new List<LoanItem>();

	// =========================================================
	// WITNESSES
	// =========================================================

	public ICollection<LoanWitness> LoanWitnesses { get; set; }
		= new List<LoanWitness>();
	// =========================================================
	// COLLATERAL
	// =========================================================

	public ICollection<Collateral> Collaterals { get; set; }
		= new List<Collateral>();
	public ICollection<Attachment> Attachments { get; set; }
	= new List<Attachment>();
}
