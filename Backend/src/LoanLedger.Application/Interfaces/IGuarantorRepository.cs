using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IGuarantorRepository
{
    Task AddAsync(Guarantor guarantor);

    Task<Guarantor?> GetByIdAsync(
        Guid userId,
        Guid guarantorId);

    Task<List<Guarantor>> GetByUserAsync(
        Guid userId);

    Task<Guarantor?> GetByPhoneAsync(
        Guid userId,
        string phoneNumber);

    Task SaveChangesAsync();
}