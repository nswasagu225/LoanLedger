using LoanLedger.Application.Categories;

namespace LoanLedger.Application.Interfaces;

public interface ICategoryService
{
Task<CreateCategoryResponse> CreateAsync(
Guid userId,
CreateCategoryRequest request);

Task<List<CategoryDto>> GetByUserAsync(
    Guid userId);

}
