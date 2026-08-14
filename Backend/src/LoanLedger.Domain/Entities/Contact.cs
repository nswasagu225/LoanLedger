using LoanLedger.Domain.Common;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Domain.Entities;

public class Contact : AuditableEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? BusinessName { get; set; }

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? Notes { get; set; }

    public ContactType ContactType { get; set; }

    public bool IsFavorite { get; set; }

    public bool IsArchived { get; set; }

    public string? ProfilePhoto { get; set; }

    public User User { get; set; } = null!;
	public ICollection<Loan> Loans { get; set; }
    = new List<Loan>();
}