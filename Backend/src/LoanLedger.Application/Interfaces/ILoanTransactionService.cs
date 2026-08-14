using LoanLedger.Application.LoanTransactions;

namespace LoanLedger.Application.Interfaces;

public interface ILoanTransactionService
{
Task<Guid> CreateAsync(
Guid userId,
CreateLoanTransactionRequest request);

Task<List<LoanTransactionResponse>> GetAllAsync(
    Guid userId);

Task<LoanTransactionResponse?> GetByIdAsync(
    Guid userId,
    Guid transactionId);

Task<List<LoanTransactionResponse>> GetTransactionsByLoanAsync(
    Guid userId,
    Guid loanId);

Task<bool> UpdateAsync(
    Guid userId,
    Guid transactionId,
    UpdateLoanTransactionRequest request);

Task DeleteTransactionAsync(
    Guid userId,
    Guid transactionId);

}
