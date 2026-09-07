namespace LoanLedger.Application.LoanWitnesses;

public class AddLoanWitnessRequest
{
    public Guid WitnessId { get; set; }

    public int? WitnessOrder { get; set; }
}