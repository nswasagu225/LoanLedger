using System.Security.Claims;
using LoanLedger.Application.Collaterals;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/collaterals")]
[Authorize]
public class CollateralsController : ControllerBase
{
    private readonly ICollateralService _service;

    public CollateralsController(
        ICollateralService service)
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
    // CREATE COLLATERAL
    // POST: api/collaterals
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCollateralRequest request)
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
            var collateral =
                await _service.CreateAsync(
                    userId.Value,
                    request);

            return Ok(new
            {
                success = true,
                message = "Collateral created successfully.",
                data = collateral
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
    // GET ALL COLLATERALS
    // GET: api/collaterals
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
            var collaterals =
                await _service.GetByUserAsync(
                    userId.Value);

            return Ok(new
            {
                success = true,
                data = collaterals
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
    // GET COLLATERALS FOR ONE LOAN
    // GET: api/collaterals/loan/{loanId}
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
            var collaterals =
                await _service.GetByLoanAsync(
                    userId.Value,
                    loanId);

            return Ok(new
            {
                success = true,
                data = collaterals
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
    // GET ONE COLLATERAL
    // GET: api/collaterals/{id}
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
            var collateral =
                await _service.GetByIdAsync(
                    userId.Value,
                    id);

            if (collateral == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Collateral not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = collateral
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
    // UPDATE COLLATERAL
    // PUT: api/collaterals/{id}
    // =========================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateCollateralRequest request)
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
                    message = "Collateral not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Collateral updated successfully."
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
    // RELEASE COLLATERAL
    // POST: api/collaterals/{id}/release
    // =========================================================

    [HttpPost("{id:guid}/release")]
    public async Task<IActionResult> Release(
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
            var released =
                await _service.ReleaseAsync(
                    userId.Value,
                    id);

            if (!released)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Collateral not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Collateral released successfully."
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