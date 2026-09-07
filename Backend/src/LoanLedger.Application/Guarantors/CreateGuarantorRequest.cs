namespace LoanLedger.Application.Guarantors;

public class CreateGuarantorRequest
{
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Relationship { get; set; }

    public string? IdentificationType { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }
}