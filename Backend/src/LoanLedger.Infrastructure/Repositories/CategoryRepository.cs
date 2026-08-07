using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly LoanLedgerDbContext _context;

    public CategoryRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(LoanCategory category)
    {
        await _context.LoanCategories.AddAsync(category);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<LoanCategory>> GetByUserAsync(Guid userId)
    {
        return await _context.LoanCategories
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.Name)
            .ToListAsync();
    }

    public async Task<LoanCategory?> GetByNameAsync(Guid userId, string name)
    {
        return await _context.LoanCategories
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.Name == name);
    }
}