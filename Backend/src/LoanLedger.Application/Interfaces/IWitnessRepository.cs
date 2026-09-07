using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IWitnessRepository
{
    Task AddAsync(Witness witness);

    Task<Witness?> GetByIdAsync(Guid id);

    Task<List<Witness>> GetByUserAsync(Guid userId);

    Task SaveChangesAsync();
}