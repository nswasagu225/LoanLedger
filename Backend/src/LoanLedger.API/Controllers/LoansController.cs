using System.Security.Claims;
using LoanLedger.Application.Interfaces;
using LoanLedger.Application.Loans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LoansController : ControllerBase
{
private readonly ILoanService _service;

public LoansController(ILoanService service)
{
    _service = service;
}

// ---------------------------------------------------------
// GET CURRENT USER ID FROM JWT
// ---------------------------------------------------------

private Guid? GetCurrentUserId()
{
    var userIdClaim =
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User.FindFirstValue(ClaimTypes.Name);

    if (Guid.TryParse(userIdClaim, out var userId))
        return userId;

    return null;
}

// ---------------------------------------------------------
// CREATE LOAN
// POST: api/loans
// ---------------------------------------------------------

[HttpPost]
public async Task<IActionResult> Create(
    CreateLoanRequest request)
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
        var result = await _service.CreateAsync(
            userId.Value,
            request);

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
// GET LOAN BY ID
// GET: api/loans/{loanId}
// =========================================================

[HttpGet("{loanId:guid}")]
public async Task<IActionResult> GetById(
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

    var loan =
        await _service.GetByIdAsync(
            userId.Value,
            loanId);

    if (loan == null)
    {
        return NotFound(new
        {
            success = false,
            message = "Loan not found."
        });
    }

    return Ok(loan);
}

// ---------------------------------------------------------
// GET CURRENT USER'S LOANS
// GET: api/loans
// ---------------------------------------------------------

[HttpGet]
public async Task<IActionResult> GetMyLoans()
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

    var loans =
        await _service.GetByUserAsync(userId.Value);

    return Ok(loans);
}

// ---------------------------------------------------------
// GET LOAN SUMMARY
// GET: api/loans/{loanId}/summary
// ---------------------------------------------------------

[HttpGet("{loanId:guid}/summary")]
public async Task<IActionResult> GetSummary(
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

    var summary =
        await _service.GetSummaryAsync(
            userId.Value,
            loanId);

    if (summary == null)
    {
        return NotFound(new
        {
            success = false,
            message = "Loan not found."
        });
    }

    return Ok(summary);
}

// ---------------------------------------------------------
// GET LOAN STATEMENT
// GET: api/loans/{loanId}/statement
// ---------------------------------------------------------

[HttpGet("{loanId:guid}/statement")]
public async Task<IActionResult> GetStatement(
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

    var statement =
        await _service.GetStatementAsync(
            userId.Value,
            loanId);

    if (statement == null)
    {
        return NotFound(new
        {
            success = false,
            message = "Loan not found."
        });
    }

    return Ok(statement);
}

// ---------------------------------------------------------
// UPDATE LOAN
// PUT: api/loans/{loanId}
// ---------------------------------------------------------

[HttpPut("{loanId:guid}")]
public async Task<IActionResult> Update(
    Guid loanId,
    UpdateLoanRequest request)
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
                loanId,
                request);

        if (!updated)
        {
            return NotFound(new
            {
                success = false,
                message = "Loan not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Loan updated successfully."
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

// ---------------------------------------------------------
// REOPEN CLOSED LOAN
// POST: api/loans/{loanId}/reopen
// ---------------------------------------------------------

[HttpPost("{loanId:guid}/reopen")]
public async Task<IActionResult> Reopen(
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
        var reopened =
            await _service.ReopenAsync(
                userId.Value,
                loanId);

        if (!reopened)
        {
            return NotFound(new
            {
                success = false,
                message = "Loan not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Loan reopened successfully."
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
// ---------------------------------------------------------
// CLOSE LOAN
// POST: api/loans/{loanId}/close
// ---------------------------------------------------------
[HttpPost("{loanId:guid}/close")]
public async Task<IActionResult> Close(
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
        var closed =
            await _service.CloseAsync(
                userId.Value,
                loanId);

        if (!closed)
        {
            return NotFound(new
            {
                success = false,
                message = "Loan not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Loan closed successfully."
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
