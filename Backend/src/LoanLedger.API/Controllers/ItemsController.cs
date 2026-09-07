using System.Security.Claims;
using LoanLedger.Application.Interfaces;
using LoanLedger.Application.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    // =========================================================
    // GET CURRENT USER ID FROM JWT
    // =========================================================

    private Guid? GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(ClaimTypes.Name);

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        return null;
    }

    // =========================================================
    // CREATE ITEM
    // POST: api/items
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateItemRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid or missing user identity."
            });
        }

        try
        {
            var result =
                await _itemService.CreateAsync(
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
    // GET CURRENT USER'S ITEMS
    // GET: api/items
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetMyItems()
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid or missing user identity."
            });
        }

        var items =
            await _itemService.GetByUserAsync(
                userId.Value);

        return Ok(items);
    }

    // =========================================================
    // GET ITEM BY ID
    // GET: api/items/{itemId}
    // =========================================================

    [HttpGet("{itemId:guid}")]
    public async Task<IActionResult> GetById(
        Guid itemId)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid or missing user identity."
            });
        }

        var item =
            await _itemService.GetByIdAsync(
                userId.Value,
                itemId);

        if (item == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Item not found."
            });
        }

        return Ok(item);
    }
}