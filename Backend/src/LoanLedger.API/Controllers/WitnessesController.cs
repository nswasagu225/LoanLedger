using LoanLedger.Application.Interfaces;
using LoanLedger.Application.Witnesses;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/witnesses")]
public class WitnessesController : ControllerBase
{
    private readonly IWitnessService _service;

    public WitnessesController(
        IWitnessService service)
    {
        _service = service;
    }

    // =========================================================
    // CREATE
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWitnessRequest request)
    {
        var userId =
            GetUserId();

        var result =
            await _service.CreateAsync(
                userId,
                request);

        return Ok(new
        {
            success = true,
            message = "Witness created successfully.",
            data = result
        });
    }

    // =========================================================
    // GET ALL
    // =========================================================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId =
            GetUserId();

        var result =
            await _service.GetByUserAsync(
                userId);

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    // =========================================================
    // GET ONE
    // =========================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id)
    {
        var userId =
            GetUserId();

        var result =
            await _service.GetByIdAsync(
                userId,
                id);

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Witness not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = result
        });
    }

    // =========================================================
    // UPDATE
    // =========================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateWitnessRequest request)
    {
        var userId =
            GetUserId();

        var updated =
            await _service.UpdateAsync(
                userId,
                id,
                request);

        if (!updated)
        {
            return NotFound(new
            {
                success = false,
                message = "Witness not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Witness updated successfully."
        });
    }

    // =========================================================
    // USER ID
    // =========================================================

    private Guid GetUserId()
    {
        var userIdClaim =
            User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            throw new Exception(
                "User identity not found.");
        }

        return Guid.Parse(
            userIdClaim.Value);
    }
}