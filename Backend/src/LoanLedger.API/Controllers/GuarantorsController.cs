using System.Security.Claims;
using LoanLedger.Application.Guarantors;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/guarantors")]
[Authorize]
public class GuarantorsController : ControllerBase
{
    private readonly IGuarantorService _service;

    public GuarantorsController(
        IGuarantorService service)
    {
        _service = service;
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
    // CREATE GUARANTOR
    // POST: api/guarantors
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateGuarantorRequest request)
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
            var guarantor =
                await _service.CreateAsync(
                    userId.Value,
                    request);

            return Ok(new
            {
                success = true,
                message =
                    "Guarantor created successfully.",
                data = guarantor
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
    // GET ALL GUARANTORS
    // GET: api/guarantors
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
                message =
                    "Invalid or missing user identity."
            });
        }

        try
        {
            var guarantors =
                await _service.GetByUserAsync(
                    userId.Value);

            return Ok(new
            {
                success = true,
                data = guarantors
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
    // GET ONE GUARANTOR
    // GET: api/guarantors/{id}
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
                message =
                    "Invalid or missing user identity."
            });
        }

        try
        {
            var guarantor =
                await _service.GetByIdAsync(
                    userId.Value,
                    id);

            if (guarantor == null)
            {
                return NotFound(new
                {
                    success = false,
                    message =
                        "Guarantor not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = guarantor
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
    // UPDATE GUARANTOR
    // PUT: api/guarantors/{id}
    // =========================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateGuarantorRequest request)
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
                    message =
                        "Guarantor not found."
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Guarantor updated successfully."
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
    // DELETE GUARANTOR
    // DELETE: api/guarantors/{id}
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
                message =
                    "Invalid or missing user identity."
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
                    message =
                        "Guarantor not found."
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Guarantor deleted successfully."
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