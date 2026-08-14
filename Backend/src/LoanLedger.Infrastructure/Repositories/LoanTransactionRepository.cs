using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class LoanTransactionRepository : ILoanTransactionRepository
{
    private readonly LoanLedgerDbContext _context;

    public LoanTransactionRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<LoanTransaction?> GetByIdAsync(Guid id)
    {
        return await _context.LoanTransactions
            .Include(t => t.Loan)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<LoanTransaction>> GetByLoanIdAsync(Guid loanId)
    {
        return await _context.LoanTransactions
            .Where(t => t.LoanId == loanId)
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task<List<LoanTransaction>> GetAllAsync()
    {
        return await _context.LoanTransactions
            .OrderByDescending(t => t.TransactionDate)
            .ToListAsync();
    }

    public async Task AddAsync(LoanTransaction transaction)
    {
        await _context.LoanTransactions.AddAsync(transaction);
    }
	
	public async Task<bool> ReferenceExistsAsync(string referenceNumber)
	{
		return await _context.LoanTransactions
			.AnyAsync(t => t.ReferenceNumber == referenceNumber);
	}

    public void Delete(LoanTransaction transaction)
    {
        _context.LoanTransactions.Remove(transaction);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}