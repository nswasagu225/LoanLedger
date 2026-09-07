using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IItemRepository
{
    Task AddAsync(Item item);

    Task<Item?> GetByIdAsync(
        Guid userId,
        Guid itemId);

    Task<Item?> GetByNameAsync(
        Guid userId,
        string name);

    Task<List<Item>> GetByUserIdAsync(
        Guid userId);

    Task SaveChangesAsync();
}