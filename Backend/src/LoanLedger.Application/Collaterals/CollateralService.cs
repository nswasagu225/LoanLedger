using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Collaterals;

public class CollateralService : ICollateralService
{
    private readonly ICollateralRepository _repository;
    private readonly ILoanRepository _loanRepository;

    public CollateralService(
        ICollateralRepository repository,
        ILoanRepository loanRepository)
    {
        _repository = repository;
        _loanRepository = loanRepository;
    }

    // =========================================================
    // CREATE
    // =========================================================

    public async Task<CollateralResponse> CreateAsync(
        Guid userId,
        CreateCollateralRequest request)
    {
        if (request.LoanId == Guid.Empty)
            throw new Exception("Loan ID is required.");

        if (string.IsNullOrWhiteSpace(request.Type))
            throw new Exception("Collateral type is required.");

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new Exception("Collateral description is required.");

        var loan = await _loanRepository.GetByIdAsync(
            request.LoanId);

        if (loan == null || loan.UserId != userId)
            throw new Exception("Loan not found.");

        if (request.EstimatedValue.HasValue &&
            request.EstimatedValue.Value < 0)
        {
            throw new Exception(
                "Estimated value cannot be negative.");
        }

        var collateral = new Collateral
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LoanId = request.LoanId,
            Type = request.Type.Trim(),
            Description = request.Description.Trim(),
            EstimatedValue = request.EstimatedValue,
            Location = request.Location?.Trim(),
            Ownership = request.Ownership?.Trim(),
            IdentificationNumber =
                request.IdentificationNumber?.Trim(),
            Notes = request.Notes?.Trim(),
            CreatedAt = DateTime.UtcNow,
            IsReleased = false
        };

        await _repository.AddAsync(collateral);
        await _repository.SaveChangesAsync();

        return Map(collateral);
    }

    // =========================================================
    // GET ALL BY USER
    // =========================================================

    public async Task<List<CollateralResponse>> GetByUserAsync(
        Guid userId)
    {
        var collaterals =
            await _repository.GetByUserAsync(userId);

        return collaterals
            .Select(Map)
            .ToList();
    }

    // =========================================================
    // GET BY LOAN
    // =========================================================

    public async Task<List<CollateralResponse>> GetByLoanAsync(
        Guid userId,
        Guid loanId)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId);

        if (loan == null || loan.UserId != userId)
            throw new Exception("Loan not found.");

        var collaterals =
            await _repository.GetByLoanAsync(
                userId,
                loanId);

        return collaterals
            .Select(Map)
            .ToList();
    }

    // =========================================================
    // GET ONE
    // =========================================================

    public async Task<CollateralResponse?> GetByIdAsync(
        Guid userId,
        Guid id)
    {
        var collateral =
            await _repository.GetByIdAsync(id);

        if (collateral == null ||
            collateral.UserId != userId)
        {
            return null;
        }

        return Map(collateral);
    }

    // =========================================================
    // UPDATE
    // =========================================================

    public async Task<bool> UpdateAsync(
        Guid userId,
        Guid id,
        UpdateCollateralRequest request)
    {
        var collateral =
            await _repository.GetByIdAsync(id);

        if (collateral == null ||
            collateral.UserId != userId)
        {
            return false;
        }

        if (collateral.IsReleased)
            throw new Exception(
                "Released collateral cannot be updated.");

        if (string.IsNullOrWhiteSpace(request.Type))
            throw new Exception(
                "Collateral type is required.");

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new Exception(
                "Collateral description is required.");

        if (request.EstimatedValue.HasValue &&
            request.EstimatedValue.Value < 0)
        {
            throw new Exception(
                "Estimated value cannot be negative.");
        }

        collateral.Type = request.Type.Trim();
        collateral.Description =
            request.Description.Trim();
        collateral.EstimatedValue =
            request.EstimatedValue;
        collateral.Location =
            request.Location?.Trim();
        collateral.Ownership =
            request.Ownership?.Trim();
        collateral.IdentificationNumber =
            request.IdentificationNumber?.Trim();
        collateral.Notes =
            request.Notes?.Trim();
        collateral.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return true;
    }

    // =========================================================
    // RELEASE
    // =========================================================

    public async Task<bool> ReleaseAsync(
        Guid userId,
        Guid id)
    {
        var collateral =
            await _repository.GetByIdAsync(id);

        if (collateral == null ||
            collateral.UserId != userId)
        {
            return false;
        }

        if (!collateral.IsReleased)
        {
            collateral.IsReleased = true;
            collateral.ReleasedAt = DateTime.UtcNow;
            collateral.UpdatedAt = DateTime.UtcNow;

            await _repository.SaveChangesAsync();
        }

        return true;
    }

    // =========================================================
    // MAPPER
    // =========================================================

    private static CollateralResponse Map(
        Collateral collateral)
    {
        return new CollateralResponse
        {
            Id = collateral.Id,
            UserId = collateral.UserId,
            LoanId = collateral.LoanId,
            Type = collateral.Type,
            Description = collateral.Description,
            EstimatedValue = collateral.EstimatedValue,
            Location = collateral.Location,
            Ownership = collateral.Ownership,
            IdentificationNumber =
                collateral.IdentificationNumber,
            Notes = collateral.Notes,
            IsReleased = collateral.IsReleased,
            ReleasedAt = collateral.ReleasedAt,
            CreatedAt = collateral.CreatedAt,
            UpdatedAt = collateral.UpdatedAt
        };
    }
}