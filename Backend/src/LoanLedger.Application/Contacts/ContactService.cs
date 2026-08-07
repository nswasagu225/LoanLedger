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

    public async Task<CreateContactResponse> CreateAsync(CreateContactRequest request)
    {
		var existingContact =
			await _repository.GetByPhoneAsync(
				request.UserId,
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
            UserId = request.UserId,
            FullName = request.FullName,
            BusinessName = request.BusinessName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            ContactType = request.ContactType,
            IsFavorite = false,
            IsArchived = false
        };

        await _repository.AddAsync(contact);

        await _repository.SaveChangesAsync();

        return new CreateContactResponse
        {
            Success = true,
            Message = "Contact created successfully.",
            ContactId = contact.Id
        };
    }

    public async Task<List<ContactDto>> GetByUserAsync(Guid userId)
    {
        var contacts = await _repository.GetByUserIdAsync(userId);

        return contacts.Select(c => new ContactDto
        {
            Id = c.Id,
            FullName = c.FullName,
            BusinessName = c.BusinessName,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email,
            ContactType = c.ContactType,
            IsFavorite = c.IsFavorite
        }).ToList();
    }
}