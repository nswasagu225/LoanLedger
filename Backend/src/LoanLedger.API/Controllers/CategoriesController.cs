using System.Security.Claims;
using LoanLedger.Application.Categories;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
private readonly ICategoryService _service;

public CategoriesController(
    ICategoryService service)
{
    _service = service;
}

// =========================================================
// GET CURRENT USER ID FROM JWT
// =========================================================

private Guid? GetCurrentUserId()
{
    var userIdClaim =
        User.FindFirstValue(
            ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue(
            ClaimTypes.Name);

    if (Guid.TryParse(
        userIdClaim,
        out var userId))
    {
        return userId;
    }

    return null;
}

// =========================================================
// CREATE CATEGORY
// POST: api/categories
// =========================================================

[HttpPost]
public async Task<IActionResult> Create(
    CreateCategoryRequest request)
{
    var userId = GetCurrentUserId();

    if (userId == null)
    {
        return Unauthorized(new
        {
            success = false,
            message =
                "Invalid or missing user identity."
        });
    }

    try
    {
        var result =
            await _service.CreateAsync(
                userId.Value,
                request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
    catch (Exception ex)
    {
        return BadRequest(new
        {
            success = false,
            message = ex.Message
        });
    }
}

// =========================================================
// GET CURRENT USER'S CATEGORIES
// GET: api/categories
// =========================================================

[HttpGet]
public async Task<IActionResult> GetMyCategories()
{
    var userId = GetCurrentUserId();

    if (userId == null)
    {
        return Unauthorized(new
        {
            success = false,
            message =
                "Invalid or missing user identity."
        });
    }

    try
    {
        var categories =
            await _service.GetByUserAsync(
                userId.Value);

        return Ok(categories);
    }
    catch (Exception ex)
    {
        return BadRequest(new
        {
            success = false,
            message = ex.Message
        });
    }
}

}
