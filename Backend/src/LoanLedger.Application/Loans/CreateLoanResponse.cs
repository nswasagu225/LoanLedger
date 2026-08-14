namespace LoanLedger.Application.Loans;

public class CreateLoanResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid LoanId { get; set; }
}