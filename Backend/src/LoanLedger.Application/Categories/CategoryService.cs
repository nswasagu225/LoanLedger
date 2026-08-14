using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Categories;

public class CategoryService : ICategoryService
{
private readonly ICategoryRepository _repository;

public CategoryService(
    ICategoryRepository repository)
{
    _repository = repository;
}

// =========================================================
// CREATE CATEGORY
// =========================================================

public async Task<CreateCategoryResponse> CreateAsync(
    Guid userId,
    CreateCategoryRequest request)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (string.IsNullOrWhiteSpace(request.Name))
    {
        throw new Exception(
            "Category name is required.");
    }

    var existing =
        await _repository.GetByNameAsync(
            userId,
            request.Name);

    if (existing != null)
    {
        return new CreateCategoryResponse
        {
            Success = false,
            Message =
                "Category already exists."
        };
    }

    var category = new LoanCategory
    {
        Id = Guid.NewGuid(),

        // IMPORTANT:
        // The owner comes from the authenticated user,
        // not from the request body.
        UserId = userId,

        Name = request.Name,

        Color = request.Color,

        Icon = request.Icon
    };

    await _repository.AddAsync(category);

    await _repository.SaveChangesAsync();

    return new CreateCategoryResponse
    {
        Success = true,

        Message =
            "Category created successfully.",

        CategoryId = category.Id
    };
}

// =========================================================
// GET CURRENT USER'S CATEGORIES
// =========================================================

public async Task<List<CategoryDto>> GetByUserAsync(
    Guid userId)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    var categories =
        await _repository.GetByUserAsync(
            userId);

    return categories
        .Select(x => new CategoryDto
        {
            Id = x.Id,

            Name = x.Name,

            Color = x.Color,

            Icon = x.Icon
        })
        .ToList();
}

}
