using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class ContactTrustRepository : IContactTrustRepository
{
    private readonly LoanLedgerDbContext _context;

    public ContactTrustRepository(
        LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<ContactTrust?>
        GetByContactAndWorkspaceAsync(
            Guid contactId,
            Guid workspaceId)
    {
        return await _context.ContactTrusts
            .FirstOrDefaultAsync(x =>
                x.ContactId == contactId &&
                x.WorkspaceId == workspaceId);
    }

    public async Task<List<ContactTrust>>
        GetByContactIdAsync(Guid contactId)
    {
        return await _context.ContactTrusts
            .Where(x => x.ContactId == contactId)
            .OrderBy(x => x.WorkspaceId)
            .ToListAsync();
    }

    public async Task AddAsync(
        ContactTrust contactTrust)
    {
        await _context.ContactTrusts
            .AddAsync(contactTrust);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}