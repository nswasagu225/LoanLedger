using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Witnesses;

public class WitnessService : IWitnessService
{
    private readonly IWitnessRepository _repository;

    public WitnessService(
        IWitnessRepository repository)
    {
        _repository = repository;
    }

    public async Task<WitnessResponse> CreateAsync(
        Guid userId,
        CreateWitnessRequest request)
    {
        if (userId == Guid.Empty)
            throw new Exception("Invalid user identity.");

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new Exception("Witness full name is required.");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new Exception("Witness phone number is required.");

        var witness = new Witness
        {
            Id = Guid.NewGuid(),

            UserId = userId,

            FullName = request.FullName.Trim(),

            PhoneNumber = request.PhoneNumber.Trim(),

            Email = string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim(),

            Address = string.IsNullOrWhiteSpace(request.Address)
                ? null
                : request.Address.Trim(),

            Relationship = string.IsNullOrWhiteSpace(request.Relationship)
                ? null
                : request.Relationship.Trim(),

            IdentificationType =
                string.IsNullOrWhiteSpace(request.IdentificationType)
                    ? null
                    : request.IdentificationType.Trim(),

            IdentificationNumber =
                string.IsNullOrWhiteSpace(request.IdentificationNumber)
                    ? null
                    : request.IdentificationNumber.Trim(),

            Notes = string.IsNullOrWhiteSpace(request.Notes)
                ? null
                : request.Notes.Trim(),

            CreatedAt = DateTime.UtcNow,

            IsDeleted = false
        };

        await _repository.AddAsync(witness);

        await _repository.SaveChangesAsync();

        return Map(witness);
    }

    public async Task<List<WitnessResponse>> GetByUserAsync(
        Guid userId)
    {
        var witnesses =
            await _repository.GetByUserAsync(userId);

        return witnesses
            .Select(Map)
            .ToList();
    }

    public async Task<WitnessResponse?> GetByIdAsync(
        Guid userId,
        Guid witnessId)
    {
        var witness =
            await _repository.GetByIdAsync(witnessId);

        if (witness == null ||
            witness.UserId != userId)
        {
            return null;
        }

        return Map(witness);
    }

    public async Task<bool> UpdateAsync(
        Guid userId,
        Guid witnessId,
        UpdateWitnessRequest request)
    {
        var witness =
            await _repository.GetByIdAsync(witnessId);

        if (witness == null ||
            witness.UserId != userId)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new Exception("Witness full name is required.");

        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new Exception("Witness phone number is required.");

        witness.FullName =
            request.FullName.Trim();

        witness.PhoneNumber =
            request.PhoneNumber.Trim();

        witness.Email =
            string.IsNullOrWhiteSpace(request.Email)
                ? null
                : request.Email.Trim();

        witness.Address =
            string.IsNullOrWhiteSpace(request.Address)
                ? null
                : request.Address.Trim();

        witness.Relationship =
            string.IsNullOrWhiteSpace(request.Relationship)
                ? null
                : request.Relationship.Trim();

        witness.IdentificationType =
            string.IsNullOrWhiteSpace(request.IdentificationType)
                ? null
                : request.IdentificationType.Trim();

        witness.IdentificationNumber =
            string.IsNullOrWhiteSpace(request.IdentificationNumber)
                ? null
                : request.IdentificationNumber.Trim();

        witness.Notes =
            string.IsNullOrWhiteSpace(request.Notes)
                ? null
                : request.Notes.Trim();

        witness.UpdatedAt =
            DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return true;
    }

    private static WitnessResponse Map(
        Witness witness)
    {
        return new WitnessResponse
        {
            Id = witness.Id,

            UserId = witness.UserId,

            FullName = witness.FullName,

            PhoneNumber = witness.PhoneNumber,

            Email = witness.Email,

            Address = witness.Address,

            Relationship = witness.Relationship,

            IdentificationType =
                witness.IdentificationType,

            IdentificationNumber =
                witness.IdentificationNumber,

            Notes = witness.Notes,

            CreatedAt = witness.CreatedAt,

            UpdatedAt = witness.UpdatedAt
        };
    }
}