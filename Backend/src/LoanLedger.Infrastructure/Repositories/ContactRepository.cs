using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly LoanLedgerDbContext _context;

    public ContactRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Contact contact)
    {
        await _context.Contacts.AddAsync(contact);
    }

    public async Task<Contact?> GetByIdAsync(Guid id)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<Contact>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Contacts
            .Where(x => x.UserId == userId)
            .OrderBy(x => x.FullName)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
	public async Task<Contact?> GetByPhoneAsync(
		Guid userId,
		string phoneNumber)
	{
		return await _context.Contacts
			.FirstOrDefaultAsync(c =>
				c.UserId == userId &&
				c.PhoneNumber == phoneNumber);
	}
}