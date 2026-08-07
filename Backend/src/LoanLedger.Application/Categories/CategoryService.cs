using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateCategoryResponse> CreateAsync(CreateCategoryRequest request)
    {
        var existing = await _repository.GetByNameAsync(
            request.UserId,
            request.Name);

        if (existing != null)
        {
            return new CreateCategoryResponse
            {
                Success = false,
                Message = "Category already exists."
            };
        }

        var category = new LoanCategory
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Name = request.Name,
            Color = request.Color,
            Icon = request.Icon
        };

        await _repository.AddAsync(category);

        await _repository.SaveChangesAsync();

        return new CreateCategoryResponse
        {
            Success = true,
            Message = "Category created successfully.",
            CategoryId = category.Id
        };
    }

    public async Task<List<CategoryDto>> GetByUserAsync(Guid userId)
    {
        var categories =
            await _repository.GetByUserAsync(userId);

        return categories.Select(x => new CategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            Color = x.Color,
            Icon = x.Icon
        }).ToList();
    }
}