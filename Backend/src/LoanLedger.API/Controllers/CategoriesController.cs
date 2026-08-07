using LoanLedger.Application.Categories;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateCategoryRequest request)
    {
        return Ok(await _service.CreateAsync(request));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(Guid userId)
    {
        return Ok(await _service.GetByUserAsync(userId));
    }
}