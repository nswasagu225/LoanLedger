using LoanLedger.Application.Contacts;

namespace LoanLedger.Application.Interfaces;

public interface IContactService
{
Task<CreateContactResponse> CreateAsync(
Guid userId,
CreateContactRequest request);

Task<List<ContactDto>> GetByUserAsync(
    Guid userId);

}
