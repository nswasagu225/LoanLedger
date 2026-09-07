using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class LoanWitnessRepository : ILoanWitnessRepository
{
    private readonly LoanLedgerDbContext _context;

    public LoanWitnessRepository(
        LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        LoanWitness loanWitness)
    {
        await _context.LoanWitnesses
            .AddAsync(loanWitness);
    }

    public async Task<LoanWitness?> GetByIdAsync(
        Guid id)
    {
        return await _context.LoanWitnesses
            .Include(x => x.Witness)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<LoanWitness?> GetByLoanAndWitnessAsync(
        Guid loanId,
        Guid witnessId)
    {
        return await _context.LoanWitnesses
            .Include(x => x.Witness)
            .FirstOrDefaultAsync(x =>
                x.LoanId == loanId &&
                x.WitnessId == witnessId);
    }

    public async Task<List<LoanWitness>> GetByLoanAsync(
        Guid loanId)
    {
        return await _context.LoanWitnesses
            .Include(x => x.Witness)
            .Where(x => x.LoanId == loanId)
            .OrderBy(x => x.WitnessOrder)
            .ToListAsync();
    }

    public async Task<int> CountByLoanAsync(
        Guid loanId)
    {
        return await _context.LoanWitnesses
            .CountAsync(x => x.LoanId == loanId);
    }

    public async Task<bool> ExistsAsync(
        Guid loanId,
        Guid witnessId)
    {
        return await _context.LoanWitnesses
            .AnyAsync(x =>
                x.LoanId == loanId &&
                x.WitnessId == witnessId);
    }

    public async Task DeleteAsync(
        LoanWitness loanWitness)
    {
        _context.LoanWitnesses
            .Remove(loanWitness);

        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}