using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Interfaces;

public interface IContactRepository
{
    Task AddAsync(Contact contact);

    Task<Contact?> GetByIdAsync(Guid id);

    Task<List<Contact>> GetByUserIdAsync(Guid userId);

    Task SaveChangesAsync();
	Task<Contact?> GetByPhoneAsync(Guid userId, string phoneNumber);
}