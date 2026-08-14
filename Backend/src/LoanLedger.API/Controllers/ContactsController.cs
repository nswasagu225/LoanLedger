using System.Security.Claims;
using LoanLedger.Application.Contacts;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
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
        {
            return userId;
        }

        return null;
    }

    // ---------------------------------------------------------
    // CREATE CONTACT
    // POST: api/contacts
    // ---------------------------------------------------------

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateContactRequest request)
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
            // UserId comes from the authenticated JWT.
            // It is NOT supplied by the client.
            var result =
                await _contactService.CreateAsync(
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

    // ---------------------------------------------------------
    // GET CURRENT USER'S CONTACTS
    // GET: api/contacts
    // ---------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> GetMyContacts()
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

        var contacts =
            await _contactService.GetByUserAsync(
                userId.Value);

        return Ok(contacts);
    }
}

