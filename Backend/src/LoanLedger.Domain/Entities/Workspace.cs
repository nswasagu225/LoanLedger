using LoanLedger.Domain.Common;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Domain.Entities;

public class Workspace : AuditableEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = string.Empty;

    public WorkspaceType Type { get; set; }

    public string? Description { get; set; }

    public string? Currency { get; set; }

    public bool IsActive { get; set; } = true;

    // =========================================================
    // NAVIGATION
    // =========================================================

    public User User { get; set; } = null!;

    public ICollection<Loan> Loans { get; set; }
        = new List<Loan>();
	public ICollection<ContactTrust> ContactTrusts { get; set; }
		= new List<ContactTrust>();
}