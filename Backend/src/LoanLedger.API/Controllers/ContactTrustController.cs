using System.Security.Claims;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/contact-trust")]
[Authorize]
public class ContactTrustController : ControllerBase
{
    private readonly IContactTrustService _service;

    public ContactTrustController(
        IContactTrustService service)
    {
        _service = service;
    }

    // =========================================================
    // CURRENT USER
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
    // GET TRUST FOR CONTACT IN WORKSPACE
    //
    // GET:
    // api/contact-trust/{contactId}/{workspaceId}
    // =========================================================

    [HttpGet("{contactId:guid}/{workspaceId:guid}")]
    public async Task<IActionResult> GetOrCreate(
        Guid contactId,
        Guid workspaceId)
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
            var trust =
                await _service.GetOrCreateAsync(
                    userId.Value,
                    contactId,
                    workspaceId);

            return Ok(trust);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    success = false,
                    message = ex.Message
                });
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
    // GET ALL TRUST PROFILES FOR CONTACT
    //
    // GET:
    // api/contact-trust/contact/{contactId}
    // =========================================================

    [HttpGet("contact/{contactId:guid}")]
    public async Task<IActionResult> GetByContact(
        Guid contactId)
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
            var trusts =
                await _service.GetByContactAsync(
                    userId.Value,
                    contactId);

            return Ok(trusts);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    success = false,
                    message = ex.Message
                });
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
	// REFRESH TRUST SCORE
	//
	// POST:
	// api/contact-trust/{contactId}/{workspaceId}/refresh
	// =========================================================

	[HttpPost("{contactId:guid}/{workspaceId:guid}/refresh")]
	public async Task<IActionResult> Refresh(
		Guid contactId,
		Guid workspaceId)
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
			var trust =
				await _service.RefreshAsync(
					userId.Value,
					contactId,
					workspaceId);

			return Ok(trust);
		}
		catch (UnauthorizedAccessException ex)
		{
			return StatusCode(
				StatusCodes.Status403Forbidden,
				new
				{
					success = false,
					message = ex.Message
				});
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