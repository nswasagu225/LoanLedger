using System.Security.Claims;
using LoanLedger.Application.Interfaces;
using LoanLedger.Application.LoanWitnesses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/loans/{loanId:guid}/witnesses")]
[Authorize]
public class LoanWitnessesController : ControllerBase
{
    private readonly ILoanWitnessService _service;

    public LoanWitnessesController(
        ILoanWitnessService service)
    {
        _service = service;
    }

    // =========================================================
    // GET CURRENT USER
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
    // ADD WITNESS TO LOAN
    // POST: api/loans/{loanId}/witnesses
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Add(
        Guid loanId,
        [FromBody] AddLoanWitnessRequest request)
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
                await _service.AddAsync(
                    userId.Value,
                    loanId,
                    request);

            return Ok(new
            {
                success = true,
                message =
                    "Witness attached to loan successfully.",
                data = result
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
    // GET LOAN WITNESSES
    // GET: api/loans/{loanId}/witnesses
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid loanId)
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
                await _service.GetByLoanAsync(
                    userId.Value,
                    loanId);

            return Ok(new
            {
                success = true,
                data = result
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
    // REMOVE WITNESS FROM LOAN
    // DELETE: api/loans/{loanId}/witnesses/{witnessId}
    // =========================================================

    [HttpDelete("{witnessId:guid}")]
    public async Task<IActionResult> Remove(
        Guid loanId,
        Guid witnessId)
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
            var removed =
                await _service.RemoveAsync(
                    userId.Value,
                    loanId,
                    witnessId);

            if (!removed)
            {
                return NotFound(new
                {
                    success = false,
                    message =
                        "Loan witness relationship not found."
                });
            }

            return Ok(new
            {
                success = true,
                message =
                    "Witness removed from loan successfully."
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