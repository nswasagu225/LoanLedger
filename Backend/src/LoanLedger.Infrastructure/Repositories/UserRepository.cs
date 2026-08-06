using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;
using LoanLedger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LoanLedger.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LoanLedgerDbContext _context;

    public UserRepository(LoanLedgerDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetByPhoneAsync(string phoneNumber)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
	public async Task<User?> GetByEmailOrPhoneAsync(string value)
	{
		return await _context.Users
			.FirstOrDefaultAsync(x =>
				x.Email == value ||
				x.PhoneNumber == value);
	}
}