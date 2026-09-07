using LoanLedger.Application.Guarantors;

namespace LoanLedger.Application.Interfaces;

public interface IGuarantorService
{
    Task<GuarantorResponse> CreateAsync(
        Guid userId,
        CreateGuarantorRequest request);

    Task<List<GuarantorResponse>> GetByUserAsync(
        Guid userId);

    Task<GuarantorResponse?> GetByIdAsync(
        Guid userId,
        Guid guarantorId);

    Task<bool> UpdateAsync(
        Guid userId,
        Guid guarantorId,
        UpdateGuarantorRequest request);

    Task<bool> DeleteAsync(
        Guid userId,
        Guid guarantorId);
}