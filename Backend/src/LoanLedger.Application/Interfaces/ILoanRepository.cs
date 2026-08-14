using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface ILoanRepository
{
    Task AddAsync(Loan loan);

    Task<Loan?> GetByIdAsync(Guid id);

    Task<List<Loan>> GetByUserAsync(Guid userId);

    Task SaveChangesAsync();
}