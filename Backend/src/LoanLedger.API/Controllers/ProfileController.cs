using System.Security.Claims;
using LoanLedger.Application.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanLedger.API.Controllers;

[ApiController]
[Route("api/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(
        IProfileService profileService)
    {
        _profileService = profileService;
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

        return Guid.TryParse(
            userIdClaim,
            out var id)
                ? id
                : null;
    }

    // =========================================================
    // GET PROFILE
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
                message = "Invalid or missing user identity."
            });
        }

        var profile =
            await _profileService.GetProfileAsync(
                userId.Value);

        if (profile == null)
        {
            return NotFound(new
            {
                success = false,
                message = "User profile not found."
            });
        }

        return Ok(new
        {
            success = true,
            data = profile
        });
    }

    // =========================================================
    // UPDATE PROFILE
    // PUT: api/profile
    // =========================================================

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileRequest request)
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
            var profile =
                await _profileService.UpdateProfileAsync(
                    userId.Value,
                    request);

            if (profile == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User profile not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Profile updated successfully.",
                data = profile
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
    // UPLOAD PROFILE IMAGE
    // POST: api/profile/image
    // =========================================================

    [HttpPost("image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfileImage(
        [FromForm] IFormFile file)
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
                message = "Profile image is required."
            });
        }

        try
        {
            await using var stream =
                file.OpenReadStream();

            var profile =
                await _profileService.UploadProfileImageAsync(
                    userId.Value,
                    stream,
                    file.FileName,
                    file.ContentType,
                    file.Length);

            if (profile == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User profile not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Profile image uploaded successfully.",
                data = profile
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
    // DELETE PROFILE IMAGE
    // DELETE: api/profile/image
    // =========================================================

    [HttpDelete("image")]
    public async Task<IActionResult> DeleteProfileImage()
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
                await _profileService.DeleteProfileImageAsync(
                    userId.Value);

            if (!deleted)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User profile not found."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Profile image deleted successfully."
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