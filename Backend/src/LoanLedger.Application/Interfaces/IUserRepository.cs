using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetByPhoneAsync(string phoneNumber);

    Task AddAsync(User user);

    Task SaveChangesAsync();
	Task<User?> GetByEmailOrPhoneAsync(string value);
}