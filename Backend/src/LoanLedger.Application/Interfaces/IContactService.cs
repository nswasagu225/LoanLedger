using LoanLedger.Application.Contacts;

namespace LoanLedger.Application.Interfaces;

public interface IContactService
{
    Task<CreateContactResponse> CreateAsync(
        Guid userId,
        CreateContactRequest request);

    Task<List<ContactDto>> GetByUserAsync(
        Guid userId);

    Task<List<ContactDto>> GetArchivedByUserAsync(
        Guid userId);

    Task<ContactDto?> GetByIdAsync(
        Guid userId,
        Guid contactId);

    Task<ContactDto?> UpdateAsync(
        Guid userId,
        Guid contactId,
        UpdateContactRequest request);

    Task<bool> FavoriteAsync(
        Guid userId,
        Guid contactId);

    Task<bool> UnfavoriteAsync(
        Guid userId,
        Guid contactId);

    Task<bool> ArchiveAsync(
        Guid userId,
        Guid contactId);

    Task<bool> UnarchiveAsync(
        Guid userId,
        Guid contactId);
}