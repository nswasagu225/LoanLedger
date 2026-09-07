using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class WitnessRepository : IWitnessRepository
{
    private readonly LoanLedgerDbContext _context;

    public WitnessRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Witness witness)
    {
        await _context.Witnesses.AddAsync(witness);
    }

    public async Task<Witness?> GetByIdAsync(Guid id)
    {
        return await _context.Witnesses
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                !x.IsDeleted);
    }

    public async Task<List<Witness>> GetByUserAsync(Guid userId)
    {
        return await _context.Witnesses
            .Where(x =>
                x.UserId == userId &&
                !x.IsDeleted)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}