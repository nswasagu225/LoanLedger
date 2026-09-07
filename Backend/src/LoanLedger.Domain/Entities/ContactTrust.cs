using LoanLedger.Domain.Enums;

namespace LoanLedger.Domain.Entities;

public class ContactTrust
{
    public Guid Id { get; set; }

    // =========================================================
    // CONTACT
    // =========================================================

    public Guid ContactId { get; set; }

    public Contact Contact { get; set; } = null!;

    // =========================================================
    // WORKSPACE
    // =========================================================

    public Guid WorkspaceId { get; set; }

    public Workspace Workspace { get; set; } = null!;

    // =========================================================
    // TRUST SCORE
    // =========================================================

    public decimal Score { get; set; }

    // =========================================================
    // NAVIGATION / AUDIT
    // =========================================================

    public DateTime UpdatedAt { get; set; }
}