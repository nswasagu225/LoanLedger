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
			.Include(x => x.LoanItems)
			.FirstOrDefaultAsync(x => x.Id == id);
	}
	public async Task<List<Loan>> GetByUserAndWorkspaceAsync(
		Guid userId,
		Guid workspaceId)
	{
		return await _context.Loans
			.Include(x => x.Contact)
			.Include(x => x.LoanCategory)
			.Include(x => x.LoanItems)
			.Where(x =>
				x.UserId == userId &&
				x.WorkspaceId == workspaceId)
			.ToListAsync();
	}

    public async Task<List<Loan>> GetByUserAsync(Guid userId)
    {
        return await _context.Loans
			.Include(x => x.Contact)
			.Include(x => x.LoanCategory)
			.Include(x => x.LoanItems)
			.Where(x => x.UserId == userId)
			.ToListAsync();
    }
	public async Task<List<Loan>> GetByContactIdAsync(
		Guid contactId)
	{
		return await _context.Loans
			.Include(x => x.LoanCategory)
			.Include(x => x.LoanItems)
			.Where(x =>
				x.ContactId == contactId)
			.ToListAsync();
	}
	public async Task ReplaceItemsAsync(
		Guid loanId,
		List<LoanItem> newItems)
	{
		// Remove existing database records directly.
		await _context.LoanItems
			.Where(x => x.LoanId == loanId)
			.ExecuteDeleteAsync();

		// Add the replacement items.
		if (newItems.Count > 0)
		{
			await _context.LoanItems.AddRangeAsync(newItems);
		}
	}

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}