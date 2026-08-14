using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly LoanLedgerDbContext _context;

    public LoanRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Loan loan)
    {
        await _context.Loans.AddAsync(loan);
    }

    public async Task<Loan?> GetByIdAsync(Guid id)
    {
        return await _context.Loans
            .Include(x => x.Contact)
            .Include(x => x.LoanCategory)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Loan>> GetByUserAsync(Guid userId)
    {
        return await _context.Loans
            .Include(x => x.Contact)
            .Include(x => x.LoanCategory)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}