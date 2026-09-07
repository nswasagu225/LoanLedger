using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface ICollateralRepository
{
    Task AddAsync(Collateral collateral);

    Task<Collateral?> GetByIdAsync(Guid id);

    Task<List<Collateral>> GetByUserAsync(Guid userId);

    Task<List<Collateral>> GetByLoanAsync(Guid userId, Guid loanId);

    Task SaveChangesAsync();
}