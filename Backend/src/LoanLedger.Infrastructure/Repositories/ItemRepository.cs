using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly LoanLedgerDbContext _context;

    public ItemRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Item item)
    {
        await _context.Items.AddAsync(item);
    }

    public async Task<Item?> GetByIdAsync(
        Guid userId,
        Guid itemId)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i =>
                i.Id == itemId &&
                i.UserId == userId &&
                !i.IsDeleted);
    }

    public async Task<Item?> GetByNameAsync(
        Guid userId,
        string name)
    {
        return await _context.Items
            .FirstOrDefaultAsync(i =>
                i.UserId == userId &&
                i.Name == name &&
                !i.IsDeleted);
    }

    public async Task<List<Item>> GetByUserIdAsync(
        Guid userId)
    {
        return await _context.Items
            .Where(i =>
                i.UserId == userId &&
                !i.IsDeleted)
            .OrderBy(i => i.Name)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}