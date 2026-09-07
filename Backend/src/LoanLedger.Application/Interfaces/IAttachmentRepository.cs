using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IAttachmentRepository
{
    Task AddAsync(Attachment attachment);

    Task<Attachment?> GetByIdAsync(Guid id);

    Task<List<Attachment>> GetByUserAsync(Guid userId);

    Task<List<Attachment>> GetByLoanAsync(
        Guid userId,
        Guid loanId);

    Task DeleteAsync(Attachment attachment);

    Task SaveChangesAsync();
}