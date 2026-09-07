using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IContactTrustRepository
{
    Task<ContactTrust?> GetByContactAndWorkspaceAsync(
        Guid contactId,
        Guid workspaceId);

    Task<List<ContactTrust>> GetByContactIdAsync(
        Guid contactId);

    Task AddAsync(ContactTrust contactTrust);

    Task SaveChangesAsync();
}