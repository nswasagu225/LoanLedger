using LoanLedger.Application.Contacts;

namespace LoanLedger.Application.Interfaces;

public interface IContactService
{
    Task<CreateContactResponse> CreateAsync(CreateContactRequest request);

    Task<List<ContactDto>> GetByUserAsync(Guid userId);
}