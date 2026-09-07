using LoanLedger.Application.Workspaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/workspaces")]
[Authorize]
public class WorkspacesController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspacesController(
        IWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    // =========================================================
    // CREATE WORKSPACE
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWorkspaceRequest request)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            var workspace =
                await _workspaceService.CreateAsync(
                    userId.Value,
                    request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = workspace.Id },
                workspace);
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
    // GET CURRENT USER WORKSPACES
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        var workspaces =
            await _workspaceService.GetAllAsync(
                userId.Value);

        return Ok(workspaces);
    }

    // =========================================================
    // GET WORKSPACE BY ID
    // =========================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var userId = GetCurrentUserId();

        if (userId == null)
            return Unauthorized();

        try
        {
            var workspace =
                await _workspaceService.GetByIdAsync(
                    userId.Value,
                    id);

            if (workspace == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Workspace not found."
                });
            }

            return Ok(workspace);
        }
        catch (UnauthorizedAccessException)
		{
			return Forbid();
		}
    }
	// =========================================================
	// UPDATE WORKSPACE
	// PUT: api/workspaces/{id}
	// =========================================================

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdateWorkspaceRequest request)
	{
		var userId = GetCurrentUserId();

		if (userId == null)
			return Unauthorized();

		try
		{
			var workspace =
				await _workspaceService.UpdateAsync(
					userId.Value,
					id,
					request);

			if (workspace == null)
			{
				return NotFound(new
				{
					success = false,
					message = "Workspace not found."
				});
			}

			return Ok(new
			{
				success = true,
				message = "Workspace updated successfully.",
				data = workspace
			});
		}
		catch (UnauthorizedAccessException)
		{
			return Forbid();
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
	// ACTIVATE WORKSPACE
	// PATCH: api/workspaces/{id}/activate
	// =========================================================

	[HttpPatch("{id:guid}/activate")]
	public async Task<IActionResult> Activate(Guid id)
	{
		var userId = GetCurrentUserId();

		if (userId == null)
			return Unauthorized();

		try
		{
			var activated =
				await _workspaceService.ActivateAsync(
					userId.Value,
					id);

			if (!activated)
			{
				return NotFound(new
				{
					success = false,
					message = "Workspace not found."
				});
			}

			return Ok(new
			{
				success = true,
				message = "Workspace activated successfully."
			});
		}
		catch (UnauthorizedAccessException)
		{
			return Forbid();
		}
	}

	// =========================================================
	// DEACTIVATE WORKSPACE
	// PATCH: api/workspaces/{id}/deactivate
	// =========================================================

	[HttpPatch("{id:guid}/deactivate")]
	public async Task<IActionResult> Deactivate(Guid id)
	{
		var userId = GetCurrentUserId();

		if (userId == null)
			return Unauthorized();

		try
		{
			var deactivated =
				await _workspaceService.DeactivateAsync(
					userId.Value,
					id);

			if (!deactivated)
			{
				return NotFound(new
				{
					success = false,
					message = "Workspace not found."
				});
			}

			return Ok(new
			{
				success = true,
				message = "Workspace deactivated successfully."
			});
		}
		catch (UnauthorizedAccessException)
		{
			return Forbid();
		}
	}

	// =========================================================
	// DELETE WORKSPACE
	// DELETE: api/workspaces/{id}
	// =========================================================

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var userId = GetCurrentUserId();

		if (userId == null)
			return Unauthorized();

		try
		{
			var deleted =
				await _workspaceService.DeleteAsync(
					userId.Value,
					id);

			if (!deleted)
			{
				return NotFound(new
				{
					success = false,
					message = "Workspace not found."
				});
			}

			return Ok(new
			{
				success = true,
				message = "Workspace deleted successfully."
			});
		}
		catch (UnauthorizedAccessException)
		{
			return Forbid();
		}
	}

    // =========================================================
    // CURRENT USER ID
    // =========================================================

    private Guid? GetCurrentUserId()
    {
        var userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim))
            return null;

        if (!Guid.TryParse(
                userIdClaim,
                out var userId))
            return null;

        return userId;
    }
}