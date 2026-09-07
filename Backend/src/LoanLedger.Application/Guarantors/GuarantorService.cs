using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Guarantors;

public class GuarantorService : IGuarantorService
{
    private readonly IGuarantorRepository _guarantorRepository;

    public GuarantorService(
        IGuarantorRepository guarantorRepository)
    {
        _guarantorRepository = guarantorRepository;
    }

    // =========================================================
    // CREATE GUARANTOR
    // =========================================================

    public async Task<GuarantorResponse> CreateAsync(
        Guid userId,
        CreateGuarantorRequest request)
    {
        if (userId == Guid.Empty)
        {
            throw new Exception(
                "Invalid user identity.");
        }

        if (request == null)
        {
            throw new Exception(
                "Guarantor information is required.");
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            throw new Exception(
                "Guarantor full name is required.");
        }

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            throw new Exception(
                "Guarantor phone number is required.");
        }

        var guarantor = new Guarantor
        {
            Id = Guid.NewGuid(),

            UserId = userId,

            FullName =
                request.FullName.Trim(),

            PhoneNumber =
                request.PhoneNumber.Trim(),

            Email =
                string.IsNullOrWhiteSpace(request.Email)
                    ? null
                    : request.Email.Trim(),

            Address =
                string.IsNullOrWhiteSpace(request.Address)
                    ? null
                    : request.Address.Trim(),

            Relationship =
                string.IsNullOrWhiteSpace(
                    request.Relationship)
                    ? null
                    : request.Relationship.Trim(),

            IdentificationType =
                string.IsNullOrWhiteSpace(
                    request.IdentificationType)
                    ? null
                    : request.IdentificationType.Trim(),

            IdentificationNumber =
                string.IsNullOrWhiteSpace(
                    request.IdentificationNumber)
                    ? null
                    : request.IdentificationNumber.Trim(),

            Notes =
                string.IsNullOrWhiteSpace(request.Notes)
                    ? null
                    : request.Notes.Trim(),

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _guarantorRepository.AddAsync(
            guarantor);

        await _guarantorRepository.SaveChangesAsync();

        return MapToResponse(guarantor);
    }

    // =========================================================
    // GET ALL GUARANTORS
    // =========================================================

    public async Task<List<GuarantorResponse>> GetByUserAsync(
        Guid userId)
    {
        var guarantors =
            await _guarantorRepository
                .GetByUserAsync(userId);

        return guarantors
            .Select(MapToResponse)
            .ToList();
    }

    // =========================================================
    // GET ONE GUARANTOR
    // =========================================================

    public async Task<GuarantorResponse?> GetByIdAsync(
        Guid userId,
        Guid guarantorId)
    {
        var guarantor =
            await _guarantorRepository.GetByIdAsync(
				userId,
				guarantorId);

        if (guarantor == null ||
            guarantor.UserId != userId ||
            guarantor.IsDeleted)
        {
            return null;
        }

        return MapToResponse(guarantor);
    }

    // =========================================================
    // UPDATE GUARANTOR
    // =========================================================

    public async Task<bool> UpdateAsync(
        Guid userId,
        Guid guarantorId,
        UpdateGuarantorRequest request)
    {
        if (request == null)
        {
            throw new Exception(
                "Guarantor information is required.");
        }

        if (string.IsNullOrWhiteSpace(
            request.FullName))
        {
            throw new Exception(
                "Guarantor full name is required.");
        }

        if (string.IsNullOrWhiteSpace(
            request.PhoneNumber))
        {
            throw new Exception(
                "Guarantor phone number is required.");
        }

        var guarantor =
            await _guarantorRepository.GetByIdAsync(
				userId,
				guarantorId);

        if (guarantor == null ||
            guarantor.UserId != userId ||
            guarantor.IsDeleted)
        {
            return false;
        }

        guarantor.FullName =
            request.FullName.Trim();

        guarantor.PhoneNumber =
            request.PhoneNumber.Trim();

        guarantor.Email =
            string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

        guarantor.Address =
            string.IsNullOrWhiteSpace(request.Address)
                ? null
                : request.Address.Trim();

        guarantor.Relationship =
            string.IsNullOrWhiteSpace(
                request.Relationship)
                ? null
                : request.Relationship.Trim();

        guarantor.IdentificationType =
            string.IsNullOrWhiteSpace(
                request.IdentificationType)
                ? null
                : request.IdentificationType.Trim();

        guarantor.IdentificationNumber =
            string.IsNullOrWhiteSpace(
                request.IdentificationNumber)
                ? null
                : request.IdentificationNumber.Trim();

        guarantor.Notes =
            string.IsNullOrWhiteSpace(request.Notes)
                ? null
                : request.Notes.Trim();

        guarantor.UpdatedAt =
            DateTime.UtcNow;

        await _guarantorRepository
            .SaveChangesAsync();

        return true;
    }

    // =========================================================
    // DELETE GUARANTOR
    // =========================================================

    public async Task<bool> DeleteAsync(
        Guid userId,
        Guid guarantorId)
    {
        var guarantor =
            await _guarantorRepository.GetByIdAsync(
				userId,
				guarantorId);

        if (guarantor == null ||
            guarantor.UserId != userId ||
            guarantor.IsDeleted)
        {
            return false;
        }

        // Soft delete.
        guarantor.IsDeleted = true;
        guarantor.UpdatedAt = DateTime.UtcNow;

        await _guarantorRepository
            .SaveChangesAsync();

        return true;
    }

    // =========================================================
    // MAP
    // =========================================================

    private static GuarantorResponse MapToResponse(
        Guarantor guarantor)
    {
        return new GuarantorResponse
        {
            Id = guarantor.Id,

            UserId = guarantor.UserId,

            FullName =
                guarantor.FullName,

            PhoneNumber =
                guarantor.PhoneNumber,

            Email =
                guarantor.Email,

            Address =
                guarantor.Address,

            Relationship =
                guarantor.Relationship,

            IdentificationType =
                guarantor.IdentificationType,

            IdentificationNumber =
                guarantor.IdentificationNumber,

            Notes =
                guarantor.Notes,

            CreatedAt =
                guarantor.CreatedAt,

            UpdatedAt =
                guarantor.UpdatedAt
        };
    }
}