using System.Security.Claims;
using LoanLedger.Application.Interfaces;
using LoanLedger.Application.LoanTransactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/loan-transactions")]
[Authorize]
public class LoanTransactionsController : ControllerBase
{
private readonly ILoanTransactionService _service;
private readonly ILoanRepository _loanRepository;
private readonly ILoanTransactionRepository _transactionRepository;
private readonly IUnitOfWork _unitOfWork;

public LoanTransactionsController(
	ILoanTransactionService service,
    ILoanRepository loanRepository,
    ILoanTransactionRepository transactionRepository,
    IUnitOfWork unitOfWork)
{
	_service = service;
    _loanRepository = loanRepository;
    _transactionRepository = transactionRepository;
    _unitOfWork = unitOfWork;
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
        return userId;

    return null;
}

// =========================================================
// CREATE TRANSACTION
// POST: api/loan-transactions
// =========================================================

[HttpPost]
public async Task<IActionResult> Create(
    CreateLoanTransactionRequest request)
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
        var transactionId =
            await _service.CreateAsync(
                userId.Value,
                request);

        return Ok(new
        {
            success = true,
            message = "Transaction created successfully.",
            transactionId
        });
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
// GET CURRENT USER'S TRANSACTIONS
// GET: api/loan-transactions
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

    var transactions =
        await _service.GetAllAsync(
            userId.Value);

    return Ok(transactions);
}

// =========================================================
// GET TRANSACTION BY ID
// GET: api/loan-transactions/{id}
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

    var transaction =
        await _service.GetByIdAsync(
            userId.Value,
            id);

    if (transaction == null)
    {
        return NotFound(new
        {
            success = false,
            message = "Transaction not found."
        });
    }

    return Ok(transaction);
}

// =========================================================
// GET TRANSACTIONS FOR A LOAN
// GET: api/loan-transactions/loan/{loanId}
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
        var transactions =
            await _service
                .GetTransactionsByLoanAsync(
                    userId.Value,
                    loanId);

        return Ok(transactions);
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
// DELETE TRANSACTION
// DELETE: api/loan-transactions/{id}
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
        await _service.DeleteTransactionAsync(
            userId.Value,
            id);

        return Ok(new
        {
            success = true,
            message =
                "Transaction deleted successfully."
        });
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
// UPDATE TRANSACTION
// PUT: api/loan-transactions/{transactionId}
// =========================================================

[HttpPut("{transactionId:guid}")]
public async Task<IActionResult> Update(
    Guid transactionId,
    UpdateLoanTransactionRequest request)
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
                transactionId,
                request);

        if (!updated)
        {
            return NotFound(new
            {
                success = false,
                message =
                    "Transaction not found."
            });
        }

        return Ok(new
        {
            success = true,
            message =
                "Transaction updated successfully."
        });
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
