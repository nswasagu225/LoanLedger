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

        ValidateContactType(request.ContactType);

        var fullName = request.FullName.Trim();
        var phoneNumber = NormalizePhoneNumber(request.PhoneNumber);

        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new Exception(
                "Contact full name is required.");
        }

        if (fullName.Length < 2)
        {
            throw new Exception(
                "Contact full name must contain at least 2 characters.");
        }

        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            throw new Exception(
                "Contact phone number is required.");
        }

        var existingContact =
            await _repository.GetByPhoneAsync(
                userId,
                phoneNumber);

        if (existingContact != null)
        {
            return new CreateContactResponse
            {
                Success = false,
                Message =
                    "A contact with this phone number already exists.",
                ContactId = existingContact.Id
            };
        }

        var contact = new Contact
        {
            Id = Guid.NewGuid(),

            UserId = userId,

            FullName = fullName,

            BusinessName = CleanOptional(
                request.BusinessName),

            PhoneNumber = phoneNumber,

            Email = CleanOptional(
                request.Email),

            Address = CleanOptional(
                request.Address),

            City = CleanOptional(
                request.City),

            State = CleanOptional(
                request.State),

            Country = CleanOptional(
                request.Country),

            Notes = CleanOptional(
                request.Notes),

            ContactType = request.ContactType,

            ProfilePhoto = CleanOptional(
                request.ProfilePhoto),

            IsFavorite = false,

            IsArchived = false,

            CreatedAt = DateTime.UtcNow
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
			.Where(c => !c.IsDeleted && !c.IsArchived)
            .Select(c => new ContactDto
            {
                Id = c.Id,

                FullName = c.FullName,

                BusinessName = c.BusinessName,

                PhoneNumber = c.PhoneNumber,

                Email = c.Email,

                ContactType = c.ContactType,

                IsFavorite = c.IsFavorite,

                ProfilePhoto = c.ProfilePhoto
            })
            .ToList();
    }
	// =========================================================
	// GET ARCHIVED CONTACTS
	// =========================================================

	public async Task<List<ContactDto>> GetArchivedByUserAsync(
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
			.Where(c => !c.IsDeleted && c.IsArchived)
			.Select(MapToDto)
			.ToList();
	}

	// =========================================================
	// GET CONTACT BY ID
	// =========================================================

	public async Task<ContactDto?> GetByIdAsync(
		Guid userId,
		Guid contactId)
	{
		if (userId == Guid.Empty)
		{
			throw new Exception(
				"Invalid user identity.");
		}

		if (contactId == Guid.Empty)
		{
			throw new Exception(
				"Invalid contact ID.");
		}

		var contact =
			await _repository.GetByIdAsync(contactId);

		if (contact == null ||
			contact.UserId != userId ||
			contact.IsDeleted)
		{
			return null;
		}

		return MapToDto(contact);
	}

	// =========================================================
// UPDATE CONTACT
// =========================================================

public async Task<ContactDto?> UpdateAsync(
    Guid userId,
    Guid contactId,
    UpdateContactRequest request)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (contactId == Guid.Empty)
    {
        throw new Exception(
            "Invalid contact ID.");
    }

    ValidateContactType(request.ContactType);

    var contact =
        await _repository.GetByIdAsync(contactId);

    if (contact == null ||
        contact.UserId != userId ||
        contact.IsDeleted)
    {
        return null;
    }

    var fullName =
        request.FullName.Trim();

    if (string.IsNullOrWhiteSpace(fullName))
    {
        throw new Exception(
            "Contact full name is required.");
    }

    if (fullName.Length < 2)
    {
        throw new Exception(
            "Contact full name must contain at least 2 characters.");
    }

    var phoneNumber =
        NormalizePhoneNumber(request.PhoneNumber);

    if (string.IsNullOrWhiteSpace(phoneNumber))
    {
        throw new Exception(
            "Contact phone number is required.");
    }

    var existingContact =
        await _repository.GetByPhoneAsync(
            userId,
            phoneNumber);

    if (existingContact != null &&
        existingContact.Id != contactId)
    {
        throw new Exception(
            "A contact with this phone number already exists.");
    }

    contact.FullName =
        fullName;

    contact.BusinessName =
        CleanOptional(
            request.BusinessName);

    contact.PhoneNumber =
        phoneNumber;

    contact.Email =
        CleanOptional(
            request.Email);

    contact.Address =
        CleanOptional(
            request.Address);

    contact.ContactType =
        request.ContactType;

    contact.ProfilePhoto =
        CleanOptional(
            request.ProfilePhoto);

    contact.UpdatedAt =
        DateTime.UtcNow;

    await _repository.SaveChangesAsync();

    return MapToDto(contact);
}
	// =========================================================
// FAVORITE CONTACT
// =========================================================

public async Task<bool> FavoriteAsync(
    Guid userId,
    Guid contactId)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (contactId == Guid.Empty)
    {
        throw new Exception(
            "Invalid contact ID.");
    }

    var contact =
        await _repository.GetByIdAsync(
            contactId);

    if (contact == null ||
        contact.UserId != userId ||
        contact.IsDeleted)
    {
        return false;
    }

    if (contact.IsArchived)
    {
        throw new Exception(
            "Archived contacts cannot be added to favorites.");
    }

    if (!contact.IsFavorite)
    {
        contact.IsFavorite = true;
        contact.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
    }

    return true;
}

// =========================================================
// UNFAVORITE CONTACT
// =========================================================

public async Task<bool> UnfavoriteAsync(
    Guid userId,
    Guid contactId)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (contactId == Guid.Empty)
    {
        throw new Exception(
            "Invalid contact ID.");
    }

    var contact =
        await _repository.GetByIdAsync(
            contactId);

    if (contact == null ||
        contact.UserId != userId ||
        contact.IsDeleted)
    {
        return false;
    }

    if (contact.IsFavorite)
    {
        contact.IsFavorite = false;
        contact.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
    }

    return true;
}
// =========================================================
// ARCHIVE CONTACT
// =========================================================

public async Task<bool> ArchiveAsync(
    Guid userId,
    Guid contactId)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (contactId == Guid.Empty)
    {
        throw new Exception(
            "Invalid contact ID.");
    }

    var contact =
        await _repository.GetByIdAsync(
            contactId);

    if (contact == null ||
        contact.UserId != userId ||
        contact.IsDeleted)
    {
        return false;
    }

    if (!contact.IsArchived)
    {
        contact.IsArchived = true;

        // An archived contact should no longer
        // remain in the favorite list.
        contact.IsFavorite = false;

        contact.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
    }

    return true;
}

// =========================================================
// UNARCHIVE CONTACT
// =========================================================

public async Task<bool> UnarchiveAsync(
    Guid userId,
    Guid contactId)
{
    if (userId == Guid.Empty)
    {
        throw new Exception(
            "Invalid user identity.");
    }

    if (contactId == Guid.Empty)
    {
        throw new Exception(
            "Invalid contact ID.");
    }

    var contact =
        await _repository.GetByIdAsync(
            contactId);

    if (contact == null ||
        contact.UserId != userId ||
        contact.IsDeleted)
    {
        return false;
    }

    if (contact.IsArchived)
    {
        contact.IsArchived = false;
        contact.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
    }

    return true;
}
	// =========================================================
	// MAPPER
	// =========================================================

	private static ContactDto MapToDto(
		Contact contact)
	{
		return new ContactDto
		{
			Id = contact.Id,
			FullName = contact.FullName,
			BusinessName = contact.BusinessName,
			PhoneNumber = contact.PhoneNumber,
			Email = contact.Email,
			ContactType = contact.ContactType,
			IsFavorite = contact.IsFavorite,
			ProfilePhoto = contact.ProfilePhoto
		};
	}

    // =========================================================
    // VALIDATE CONTACT TYPE
    // =========================================================

    private static void ValidateContactType(
        Domain.Enums.ContactType type)
    {
        if (!Enum.IsDefined(type))
        {
            throw new Exception(
                "Invalid contact type.");
        }
    }

    // =========================================================
    // CLEAN OPTIONAL INPUT
    // =========================================================

    private static string? CleanOptional(
        string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Trim();
    }

    // =========================================================
    // NORMALIZE PHONE NUMBER
    // =========================================================

    private static string NormalizePhoneNumber(
        string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        return phoneNumber
            .Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");
    }
}