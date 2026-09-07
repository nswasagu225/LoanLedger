using LoanLedger.Application.LoanWitnesses;

namespace LoanLedger.Application.Interfaces;

public interface ILoanWitnessService
{
    Task<LoanWitnessResponse> AddAsync(
        Guid userId,
        Guid loanId,
        AddLoanWitnessRequest request);

    Task<List<LoanWitnessResponse>> GetByLoanAsync(
        Guid userId,
        Guid loanId);

    Task<bool> RemoveAsync(
        Guid userId,
        Guid loanId,
        Guid witnessId);
}