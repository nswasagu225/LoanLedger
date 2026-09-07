using LoanLedger.Application.Collaterals;

namespace LoanLedger.Application.Interfaces;

public interface ICollateralService
{
    Task<CollateralResponse> CreateAsync(
        Guid userId,
        CreateCollateralRequest request);

    Task<List<CollateralResponse>> GetByUserAsync(
        Guid userId);

    Task<List<CollateralResponse>> GetByLoanAsync(
        Guid userId,
        Guid loanId);

    Task<CollateralResponse?> GetByIdAsync(
        Guid userId,
        Guid id);

    Task<bool> UpdateAsync(
        Guid userId,
        Guid id,
        UpdateCollateralRequest request);

    Task<bool> ReleaseAsync(
        Guid userId,
        Guid id);
}