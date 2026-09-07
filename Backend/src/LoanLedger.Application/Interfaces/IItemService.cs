using LoanLedger.Application.Items;

namespace LoanLedger.Application.Interfaces;

public interface IItemService
{
    Task<CreateItemResponse> CreateAsync(
        Guid userId,
        CreateItemRequest request);

    Task<List<ItemDto>> GetByUserAsync(
        Guid userId);

    Task<ItemDto?> GetByIdAsync(
        Guid userId,
        Guid itemId);
}