using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface ILoanTransactionRepository
{
    Task<LoanTransaction?> GetByIdAsync(Guid id);

    Task<List<LoanTransaction>> GetByLoanIdAsync(Guid loanId);

    Task<List<LoanTransaction>> GetAllAsync();

    Task AddAsync(LoanTransaction transaction);
	
	Task<bool> ReferenceExistsAsync(string referenceNumber);

    void Delete(LoanTransaction transaction);

    Task SaveChangesAsync();
}