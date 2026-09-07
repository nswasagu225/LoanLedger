using System.Security.Claims;
using LoanLedger.Application.Attachments;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LoanLedger.API.Models;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/attachments")]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly IAttachmentService _service;

    public AttachmentsController(
        IAttachmentService service)
    {
        _service = service;
    }

	// =========================================================
	// UPLOAD FILE
	// POST: api/attachments/upload
	// =========================================================

	[HttpPost("upload")]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> Upload(
		[FromForm] UploadAttachmentRequest request)
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

		if (request.File == null ||
			request.File.Length == 0)
		{
			return BadRequest(new
			{
				success = false,
				message = "A file is required."
			});
		}

		try
		{
			await using var stream =
				request.File.OpenReadStream();

			var attachment =
				await _service.UploadAsync(
					userId.Value,
					request.LoanId,
					stream,
					request.File.FileName,
					request.File.ContentType,
					request.AttachmentType,
					request.Description);

			return Ok(new
			{
				success = true,
				message = "Attachment uploaded successfully.",
				data = attachment
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
    // GET CURRENT USER ID
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
	// CREATE
	// POST: api/attachments
	// =========================================================

	[HttpPost]
	[Consumes("multipart/form-data")]
	public async Task<IActionResult> Create(
		[FromForm] Guid loanId,
		[FromForm] IFormFile file,
		[FromForm] string attachmentType,
		[FromForm] string? description)
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

		if (file == null || file.Length == 0)
		{
			return BadRequest(new
			{
				success = false,
				message = "A file is required."
			});
		}

		if (loanId == Guid.Empty)
		{
			return BadRequest(new
			{
				success = false,
				message = "Loan ID is required."
			});
		}

		if (string.IsNullOrWhiteSpace(attachmentType))
		{
			return BadRequest(new
			{
				success = false,
				message = "Attachment type is required."
			});
		}

		try
		{
			await using var stream =
				file.OpenReadStream();

			var attachment =
				await _service.UploadAsync(
					userId.Value,
					loanId,
					stream,
					file.FileName,
					file.ContentType,
					attachmentType,
					description);

			return Ok(new
			{
				success = true,
				message = "Attachment uploaded successfully.",
				data = attachment
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
    // GET ALL
    // GET: api/attachments
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
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
            var attachments =
                await _service.GetByUserAsync(
                    userId.Value);

            return Ok(new
            {
                success = true,
                data = attachments
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
    // GET BY LOAN
    // GET: api/attachments/loan/{loanId}
    // =========================================================

    [HttpGet("loan/{loanId:guid}")]
    public async Task<IActionResult> GetByLoan(
        Guid loanId)
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
            var attachments =
                await _service.GetByLoanAsync(
                    userId.Value,
                    loanId);

            return Ok(new
            {
                success = true,
                data = attachments
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
    // GET ONE
    // GET: api/attachments/{id}
    // =========================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
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
            var attachment =
                await _service.GetByIdAsync(
                    userId.Value,
                    id);

            if (attachment == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Attachment not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = attachment
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
	// UPDATE
	// PUT: api/attachments/{id}
	// =========================================================

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(
		Guid id,
		[FromBody] UpdateAttachmentRequest request)
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
			var updated =
				await _service.UpdateAsync(
					userId.Value,
					id,
					request);

			if (!updated)
			{
				return NotFound(new
				{
					success = false,
					message = "Attachment not found."
				});
			}

			return Ok(new
			{
				success = true,
				message = "Attachment updated successfully."
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
	// DOWNLOAD
	// GET: api/attachments/{id}/download
	// =========================================================

	[HttpGet("{id:guid}/download")]
	public async Task<IActionResult> Download(
		Guid id)
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
				await _service.DownloadAsync(
					userId.Value,
					id);

			if (result == null)
			{
				return NotFound(new
				{
					success = false,
					message = "Attachment or file not found."
				});
			}

			return File(
				result.Value.Stream,
				result.Value.ContentType,
				result.Value.FileName);
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
    // DELETE
    // DELETE: api/attachments/{id}
    // =========================================================

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id)
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
            var deleted =
                await _service.DeleteAsync(
                    userId.Value,
                    id);

            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Attachment not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Attachment deleted successfully."
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