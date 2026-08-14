using System.Security.Claims;
using LoanLedger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
private readonly IUserRepository _userRepository;

public ProfileController(
    IUserRepository userRepository)
{
    _userRepository = userRepository;
}

// =========================================================
// GET CURRENT USER ID FROM JWT
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
// GET CURRENT USER PROFILE
// GET: api/profile
// =========================================================

[HttpGet]
public async Task<IActionResult> GetProfile()
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

    var user =
        await _userRepository.GetByIdAsync(
            userId.Value);

    if (user == null)
    {
        return NotFound(new
        {
            success = false,
            message = "User account not found."
        });
    }

    return Ok(new
    {
        success = true,
        user = new
        {
            id = user.Id,

            fullName = user.FullName,

            email = user.Email,

            phoneNumber = user.PhoneNumber,

            isActive = user.IsActive,

            isEmailVerified =
                user.IsEmailVerified,

            isPhoneVerified =
                user.IsPhoneVerified,

            lastLoginAt =
                user.LastLoginAt
        }
    });
}

}
