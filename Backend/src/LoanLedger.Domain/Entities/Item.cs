using LoanLedger.Domain.Common;

namespace LoanLedger.Domain.Entities;

public class Item : AuditableEntity
{
    public Guid Id { get; set; }

    // =========================================================
    // OWNER
    // =========================================================

    public Guid UserId { get; set; }

    // =========================================================
    // ITEM INFORMATION
    // =========================================================

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Unit { get; set; }

    public decimal DefaultRate { get; set; }

    // =========================================================
    // NAVIGATION
    // =========================================================

    public User User { get; set; } = null!;

    public ICollection<LoanItem> LoanItems { get; set; }
        = new List<LoanItem>();
}