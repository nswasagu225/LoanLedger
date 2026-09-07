namespace LoanLedger.Application.LoanWitnesses;

public class LoanWitnessResponse
{
    public Guid Id { get; set; }

    public Guid LoanId { get; set; }

    public Guid WitnessId { get; set; }

    public int WitnessOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Relationship { get; set; }

    public string? IdentificationType { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Notes { get; set; }
}