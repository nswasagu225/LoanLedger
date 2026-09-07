using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class CollateralRepository : ICollateralRepository
{
    private readonly LoanLedgerDbContext _context;

    public CollateralRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Collateral collateral)
    {
        await _context.Collaterals.AddAsync(collateral);
    }

    public async Task<Collateral?> GetByIdAsync(Guid id)
    {
        return await _context.Collaterals
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Collateral>> GetByUserAsync(Guid userId)
    {
        return await _context.Collaterals
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Collateral>> GetByLoanAsync(
        Guid userId,
        Guid loanId)
    {
        return await _context.Collaterals
            .Where(x =>
                x.UserId == userId &&
                x.LoanId == loanId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}