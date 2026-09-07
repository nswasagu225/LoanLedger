using LoanLedger.Application.Witnesses;

namespace LoanLedger.Application.Interfaces;

public interface IWitnessService
{
    Task<WitnessResponse> CreateAsync(
        Guid userId,
        CreateWitnessRequest request);

    Task<List<WitnessResponse>> GetByUserAsync(
        Guid userId);

    Task<WitnessResponse?> GetByIdAsync(
        Guid userId,
        Guid witnessId);

    Task<bool> UpdateAsync(
        Guid userId,
        Guid witnessId,
        UpdateWitnessRequest request);
}