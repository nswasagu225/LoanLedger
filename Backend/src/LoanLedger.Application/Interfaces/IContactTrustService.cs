using LoanLedger.Application.Contacts;

namespace LoanLedger.Application.Interfaces;

public interface IContactTrustService
{
    Task<ContactTrustDto> GetOrCreateAsync(
        Guid userId,
        Guid contactId,
        Guid workspaceId);

    Task<List<ContactTrustDto>> GetByContactAsync(
        Guid userId,
        Guid contactId);

    Task<ContactTrustDto> RefreshAsync(
        Guid userId,
        Guid contactId,
        Guid workspaceId);

    Task RefreshForLoanAsync(
        Guid userId,
        Guid loanId);
}