namespace LoanLedger.Application.Loans;

public class UpdateLoanItemsRequest
{
    public List<CreateLoanItemRequest> Items { get; set; }
        = new();
}