using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IUserRepository
{
Task<User?> GetByIdAsync(Guid id);

Task<User?> GetByEmailAsync(string email);

Task<User?> GetByPhoneAsync(string phoneNumber);

Task<User?> GetByEmailOrPhoneAsync(string value);

Task AddAsync(User user);

Task SaveChangesAsync();

}
