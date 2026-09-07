using LoanLedger.Application.Loans;

namespace LoanLedger.Application.Interfaces;

public interface ILoanService
{
    Task<CreateLoanResponse> CreateAsync(
        Guid userId,
        CreateLoanRequest request);

    Task<bool> UpdateAsync(
        Guid userId,
        Guid loanId,
        UpdateLoanRequest request);

    Task<List<LoanResponse>> GetByUserAsync(
        Guid userId);

    Task<LoanResponse?> GetByIdAsync(
        Guid userId,
        Guid loanId);

    Task<LoanSummaryResponse?> GetSummaryAsync(
        Guid userId,
        Guid loanId);

    Task<LoanStatementResponse?> GetStatementAsync(
        Guid userId,
        Guid loanId);

    Task<bool> ReopenAsync(
        Guid userId,
        Guid loanId);
	Task<bool> CloseAsync(
		Guid userId,
		Guid loanId);
	Task<bool> UpdateItemsAsync(
		Guid userId,
		Guid loanId,
		UpdateLoanItemsRequest request);
	Task<List<LoanResponse>> GetByWorkspaceAsync(
		Guid userId,
		Guid workspaceId);
}