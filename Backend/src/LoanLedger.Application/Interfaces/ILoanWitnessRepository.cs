using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface ILoanWitnessRepository
{
    Task AddAsync(LoanWitness loanWitness);

    Task<LoanWitness?> GetByIdAsync(Guid id);

    Task<LoanWitness?> GetByLoanAndWitnessAsync(
        Guid loanId,
        Guid witnessId);

    Task<List<LoanWitness>> GetByLoanAsync(
        Guid loanId);

    Task<int> CountByLoanAsync(
        Guid loanId);

    Task<bool> ExistsAsync(
        Guid loanId,
        Guid witnessId);

    Task DeleteAsync(
        LoanWitness loanWitness);

    Task SaveChangesAsync();
}