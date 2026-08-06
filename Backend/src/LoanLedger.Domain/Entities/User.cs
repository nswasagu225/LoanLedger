using LoanLedger.Domain.Common;

namespace LoanLedger.Domain.Entities;

public class User : AuditableEntity
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
}