using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(LoanCategory category);

    Task SaveChangesAsync();

    Task<List<LoanCategory>> GetByUserAsync(Guid userId);

    Task<LoanCategory?> GetByNameAsync(Guid userId, string name);
}