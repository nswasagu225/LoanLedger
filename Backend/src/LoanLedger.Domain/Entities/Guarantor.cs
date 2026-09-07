namespace LoanLedger.Domain.Entities;

public class Guarantor
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // =========================================================
    // OWNER
    // =========================================================

    public Guid UserId { get; set; }

    public User User { get; set; } = null!;

    // =========================================================
    // GUARANTOR INFORMATION
    // =========================================================

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Relationship { get; set; }

    public string? IdentificationType { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }

    // =========================================================
    // AUDIT
    // =========================================================

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    // =========================================================
    // LOANS
    // =========================================================

    public ICollection<Loan> Loans { get; set; }
        = new List<Loan>();
}