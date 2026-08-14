using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Contacts;

public class ContactService : IContactService
{
private readonly IContactRepository _repository;

public ContactService(IContactRepository repository)
{
    _repository = repository;
}

// =========================================================
// CREATE CONTACT
// =========================================================

public async Task<CreateContactResponse> CreateAsync(
    Guid userId,
    CreateContactRequest request)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (string.IsNullOrWhiteSpace(request.FullName))
    {
        throw new Exception(
            "Contact full name is required.");
    }

    if (string.IsNullOrWhiteSpace(request.PhoneNumber))
    {
        throw new Exception(
            "Contact phone number is required.");
    }

    var existingContact =
        await _repository.GetByPhoneAsync(
            userId,
            request.PhoneNumber);

    if (existingContact != null)
    {
        return new CreateContactResponse
        {
            Success = false,
            Message = "Contact already exists."
        };
    }

    var contact = new Contact
    {
        Id = Guid.NewGuid(),

        // IMPORTANT:
        // The owner comes from the authenticated user,
        // not from the request body.
        UserId = userId,

        FullName = request.FullName,

        BusinessName =
            request.BusinessName,

        PhoneNumber =
            request.PhoneNumber,

        Email =
            request.Email,

        Address =
            request.Address,

        ContactType =
            request.ContactType,

        IsFavorite = false,

        IsArchived = false
    };

    await _repository.AddAsync(contact);

    await _repository.SaveChangesAsync();

    return new CreateContactResponse
    {
        Success = true,
        Message =
            "Contact created successfully.",
        ContactId = contact.Id
    };
}

// =========================================================
// GET CURRENT USER'S CONTACTS
// =========================================================

public async Task<List<ContactDto>> GetByUserAsync(
    Guid userId)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    var contacts =
        await _repository.GetByUserIdAsync(
            userId);

    return contacts
        .Select(c => new ContactDto
        {
            Id = c.Id,

            FullName =
                c.FullName,

            BusinessName =
                c.BusinessName,

            PhoneNumber =
                c.PhoneNumber,

            Email =
                c.Email,

            ContactType =
                c.ContactType,

            IsFavorite =
                c.IsFavorite
        })
        .ToList();
}

}
