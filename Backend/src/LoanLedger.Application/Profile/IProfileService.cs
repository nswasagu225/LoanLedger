using LoanLedger.Application.DTOs.Profile;

namespace LoanLedger.Application.Profile;

public interface IProfileService
{
    Task<ProfileDto?> GetProfileAsync(Guid userId);

    Task<ProfileDto?> UpdateProfileAsync(
        Guid userId,
        UpdateProfileRequest request);

    Task<ProfileDto?> UploadProfileImageAsync(
        Guid userId,
        Stream fileStream,
        string fileName,
        string contentType,
        long fileSize);

    Task<bool> DeleteProfileImageAsync(
        Guid userId);
}