using System.ComponentModel.DataAnnotations;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Contacts;

public class UpdateContactRequest
{
    [Required(
        ErrorMessage = "Contact full name is required.")]
    [StringLength(
        150,
        MinimumLength = 2,
        ErrorMessage = "Contact full name must be between 2 and 150 characters.")]
    public string FullName { get; set; } = string.Empty;
	
    [StringLength(
        150,
        ErrorMessage = "Business name cannot exceed 150 characters.")]
    public string? BusinessName { get; set; }

    [Required(
		ErrorMessage = "Contact phone number is required.")]
	[RegularExpression(
		@"^\+?[0-9][0-9\s\-()]{6,19}$",
		ErrorMessage = "Please enter a valid phone number.")]
	public string PhoneNumber { get; set; } = string.Empty;


    [EmailAddress(
        ErrorMessage = "Please enter a valid email address.")]
    [StringLength(
        150,
        ErrorMessage = "Email address cannot exceed 150 characters.")]
    public string? Email { get; set; }

    [StringLength(
        500,
        ErrorMessage = "Address cannot exceed 500 characters.")]
    public string? Address { get; set; }

    [EnumDataType(
        typeof(ContactType),
        ErrorMessage = "Invalid contact type.")]
    public ContactType ContactType { get; set; }

    [StringLength(
        500,
        ErrorMessage = "Profile photo reference cannot exceed 500 characters.")]
    public string? ProfilePhoto { get; set; }
}