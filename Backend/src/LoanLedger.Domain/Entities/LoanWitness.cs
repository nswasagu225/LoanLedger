namespace LoanLedger.Domain.Entities;

public class LoanWitness
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid LoanId { get; set; }

    public Loan Loan { get; set; } = null!;

    public Guid WitnessId { get; set; }

    public Witness Witness { get; set; } = null!;

    public int WitnessOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}