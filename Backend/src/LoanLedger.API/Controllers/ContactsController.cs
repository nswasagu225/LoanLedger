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

    // =========================================================
    // GET CURRENT USER ID FROM JWT
    // =========================================================

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

    // =========================================================
    // CREATE CONTACT
    // POST: api/contacts
    // =========================================================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContactRequest request)
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
                await _contactService.CreateAsync(
                    userId.Value,
                    request);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    success = false,
                    message = result.Message,
                    contactId = result.ContactId
                });
            }

            return Ok(new
            {
                success = true,
                message = result.Message,
                contactId = result.ContactId
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
    // GET CURRENT USER'S CONTACTS
    // GET: api/contacts
    // =========================================================

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

        try
        {
            var contacts =
                await _contactService.GetByUserAsync(
                    userId.Value);

            return Ok(contacts);
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
    // GET CONTACT BY ID
    // GET: api/contacts/{id}
    // =========================================================

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
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
            var contact =
                await _contactService.GetByIdAsync(
                    userId.Value,
                    id);

            if (contact == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Contact not found."
                });
            }

            return Ok(contact);
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
    // UPDATE CONTACT
    // PUT: api/contacts/{id}
    // =========================================================

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateContactRequest request)
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
            var contact =
                await _contactService.UpdateAsync(
                    userId.Value,
                    id,
                    request);

            if (contact == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Contact not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Contact updated successfully.",
                data = contact
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
    // FAVORITE CONTACT
    // PUT: api/contacts/{id}/favorite
    // =========================================================

    [HttpPut("{id:guid}/favorite")]
    public async Task<IActionResult> Favorite(Guid id)
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
                await _contactService.FavoriteAsync(
                    userId.Value,
                    id);

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Contact not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Contact added to favorites successfully."
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
    // UNFAVORITE CONTACT
    // PUT: api/contacts/{id}/unfavorite
    // =========================================================

    [HttpPut("{id:guid}/unfavorite")]
    public async Task<IActionResult> Unfavorite(Guid id)
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
                await _contactService.UnfavoriteAsync(
                    userId.Value,
                    id);

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Contact not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Contact removed from favorites successfully."
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
    // ARCHIVE CONTACT
    // PUT: api/contacts/{id}/archive
    // =========================================================

    [HttpPut("{id:guid}/archive")]
    public async Task<IActionResult> Archive(Guid id)
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
                await _contactService.ArchiveAsync(
                    userId.Value,
                    id);

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Contact not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Contact archived successfully."
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
    // UNARCHIVE CONTACT
    // PUT: api/contacts/{id}/unarchive
    // =========================================================

    [HttpPut("{id:guid}/unarchive")]
    public async Task<IActionResult> Unarchive(Guid id)
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
                await _contactService.UnarchiveAsync(
                    userId.Value,
                    id);

            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Contact not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Contact unarchived successfully."
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
    // GET ARCHIVED CONTACTS
    // GET: api/contacts/archived
    // =========================================================

    [HttpGet("archived")]
    public async Task<IActionResult> GetArchivedContacts()
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
            var contacts =
                await _contactService.GetArchivedByUserAsync(
                    userId.Value);

            return Ok(contacts);
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