using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Items;

public class ItemService : IItemService
{
    private readonly IItemRepository _repository;

    public ItemService(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateItemResponse> CreateAsync(
        Guid userId,
        CreateItemRequest request)
    {
        if (userId == Guid.Empty)
        {
            throw new Exception(
                "Invalid user identity.");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new Exception(
                "Item name is required.");
        }

        if (request.DefaultRate < 0)
        {
            throw new Exception(
                "Item rate cannot be negative.");
        }

        var existingItem =
            await _repository.GetByNameAsync(
                userId,
                request.Name);

        if (existingItem != null)
        {
            return new CreateItemResponse
            {
                Success = false,
                Message = "Item already exists."
            };
        }

        var item = new Item
        {
            Id = Guid.NewGuid(),

            UserId = userId,

            Name = request.Name.Trim(),

            Description = request.Description,

            Unit = request.Unit,

            DefaultRate = request.DefaultRate
        };

        await _repository.AddAsync(item);

        await _repository.SaveChangesAsync();

        return new CreateItemResponse
        {
            Success = true,

            Message =
                "Item created successfully.",

            ItemId = item.Id
        };
    }

    public async Task<List<ItemDto>> GetByUserAsync(
        Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new Exception(
                "Invalid user identity.");
        }

        var items =
            await _repository.GetByUserIdAsync(
                userId);

        return items
            .Select(i => new ItemDto
            {
                Id = i.Id,

                Name = i.Name,

                Description = i.Description,

                Unit = i.Unit,

                DefaultRate = i.DefaultRate
            })
            .ToList();
    }
	public async Task<ItemDto?> GetByIdAsync(
		Guid userId,
		Guid itemId)
	{
		if (userId == Guid.Empty)
		{
			throw new Exception(
				"Invalid user identity.");
		}

		var item =
			await _repository.GetByIdAsync(
				userId,
				itemId);

		if (item == null ||
			item.UserId != userId)
		{
			return null;
		}

		return new ItemDto
		{
			Id = item.Id,

			Name = item.Name,

			Description = item.Description,

			Unit = item.Unit,

			DefaultRate = item.DefaultRate
		};
	}
}