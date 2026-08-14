using LoanLedger.Domain.Common;

namespace LoanLedger.Domain.Entities;

public class User : AuditableEntity
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsEmailVerified { get; set; }

    public bool IsPhoneVerified { get; set; }

    public bool IsActive { get; set; } = true;
	
	public DateTime? LastLoginAt { get; set; }
	public ICollection<Contact> Contacts { get; set; }
    = new List<Contact>();
	public ICollection<LoanCategory> LoanCategories { get; set; }
    = new List<LoanCategory>();
	public ICollection<Loan> Loans { get; set; }
    = new List<Loan>();
}