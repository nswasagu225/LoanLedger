namespace LoanLedger.Domain.Entities;

public class Collateral
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // =========================================================
    // OWNER
    // =========================================================

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    // =========================================================
    // LOAN
    // =========================================================

    public Guid LoanId { get; set; }

    public Loan Loan { get; set; } = null!;

    // =========================================================
    // COLLATERAL INFORMATION
    // =========================================================

    public string Type { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal? EstimatedValue { get; set; }

    public string? Location { get; set; }

    public string? Ownership { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }

    // =========================================================
    // RELEASE
    // =========================================================

    public bool IsReleased { get; set; }

    public DateTime? ReleasedAt { get; set; }

    // =========================================================
    // AUDIT
    // =========================================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}