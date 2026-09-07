using LoanLedger.Application.DTOs.Profile;
using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Profile;

public class ProfileService : IProfileService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorage;

    private const long MaxProfileImageSize =
        5 * 1024 * 1024;

    private static readonly string[] AllowedExtensions =
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    };

    public ProfileService(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorage)
    {
        _unitOfWork = unitOfWork;
        _fileStorage = fileStorage;
    }

    // =========================================================
    // GET PROFILE
    // =========================================================

    public async Task<ProfileDto?> GetProfileAsync(
        Guid userId)
    {
        var user =
            await _unitOfWork.Users.GetByIdAsync(userId);

        if (user == null)
            return null;

        return Map(user);
    }

    // =========================================================
	// UPDATE PROFILE
	// =========================================================

	public async Task<ProfileDto?> UpdateProfileAsync(
		Guid userId,
		UpdateProfileRequest request)
	{
		if (string.IsNullOrWhiteSpace(request.FullName))
			throw new Exception(
				"Full name is required.");

		if (string.IsNullOrWhiteSpace(request.PhoneNumber))
			throw new Exception(
				"Phone number is required.");

		var user =
			await _unitOfWork.Users.GetByIdAsync(userId);

		if (user == null)
			return null;

		user.FullName =
			request.FullName.Trim();

		user.PhoneNumber =
			request.PhoneNumber.Trim();

		user.UpdatedAt =
			DateTime.UtcNow;

		await _unitOfWork.SaveChangesAsync();

		return Map(user);
	}

    // =========================================================
    // UPLOAD / CHANGE PROFILE IMAGE
    // =========================================================

    public async Task<ProfileDto?> UploadProfileImageAsync(
        Guid userId,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize)
    {
        if (fileStream == null)
            throw new Exception(
                "Profile image is required.");

        if (fileStream.Length == 0)
            throw new Exception(
                "Profile image is required.");

        if (fileSize <= 0)
            throw new Exception(
                "Invalid profile image size.");

        if (fileSize > MaxProfileImageSize)
            throw new Exception(
                "Profile image must not exceed 5 MB.");

        if (string.IsNullOrWhiteSpace(fileName))
            throw new Exception(
                "Profile image file name is required.");

        var extension =
            Path.GetExtension(fileName)
                .ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            throw new Exception(
                "Only JPG, JPEG, PNG, and WEBP images are allowed.");
        }

        var user =
            await _unitOfWork.Users.GetByIdAsync(userId);

        if (user == null)
            return null;

        var oldProfileImage =
            user.ProfileImage;

        var storageKey =
            await _fileStorage.SaveAsync(
                fileStream,
                fileName,
                contentType,
                "profiles");

        user.ProfileImage =
            storageKey;

        user.UpdatedAt =
            DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        // Delete the old image only after
        // database update succeeds.
        if (!string.IsNullOrWhiteSpace(oldProfileImage))
        {
            await _fileStorage.DeleteAsync(
                oldProfileImage);
        }

        return Map(user);
    }

    // =========================================================
    // DELETE PROFILE IMAGE
    // =========================================================

    public async Task<bool> DeleteProfileImageAsync(
        Guid userId)
    {
        var user =
            await _unitOfWork.Users.GetByIdAsync(userId);

        if (user == null)
            return false;

        var oldProfileImage =
            user.ProfileImage;

        if (string.IsNullOrWhiteSpace(oldProfileImage))
            return true;

        user.ProfileImage = null;

        user.UpdatedAt =
            DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        await _fileStorage.DeleteAsync(
            oldProfileImage);

        return true;
    }

    // =========================================================
    // MAPPER
    // =========================================================

    private static ProfileDto Map(
        User user)
    {
        return new ProfileDto
        {
            Id = user.Id,

            FullName =
                user.FullName,

            Email =
                user.Email,

            PhoneNumber =
                user.PhoneNumber,

            ProfileImage =
                user.ProfileImage,

            IsActive =
                user.IsActive,

            IsEmailVerified =
                user.IsEmailVerified,

            IsPhoneVerified =
                user.IsPhoneVerified,

            LastLoginAt =
                user.LastLoginAt
        };
    }
}