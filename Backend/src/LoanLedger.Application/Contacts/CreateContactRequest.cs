using System.ComponentModel.DataAnnotations;
using LoanLedger.Domain.Enums;

namespace LoanLedger.Application.Contacts;

public class CreateContactRequest
{
    [Required(ErrorMessage = "Contact full name is required.")]
    [StringLength(
        150,
        MinimumLength = 2,
        ErrorMessage = "Contact full name must be between 2 and 150 characters.")]
    public string FullName { get; set; } = string.Empty;

    [StringLength(
        150,
        ErrorMessage = "Business name cannot exceed 150 characters.")]
    public string? BusinessName { get; set; }

    [Required(ErrorMessage = "Contact phone number is required.")]
    [RegularExpression(
        @"^\+?[0-9][0-9\s\-()]{6,19}$",
        ErrorMessage = "Please enter a valid phone number.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress(
        ErrorMessage = "Please enter a valid email address.")]
    [StringLength(
        254,
        ErrorMessage = "Email address cannot exceed 254 characters.")]
    public string? Email { get; set; }

    [StringLength(
        300,
        ErrorMessage = "Address cannot exceed 300 characters.")]
    public string? Address { get; set; }

    [StringLength(
        100,
        ErrorMessage = "City cannot exceed 100 characters.")]
    public string? City { get; set; }

    [StringLength(
        100,
        ErrorMessage = "State cannot exceed 100 characters.")]
    public string? State { get; set; }

    [StringLength(
        100,
        ErrorMessage = "Country cannot exceed 100 characters.")]
    public string? Country { get; set; }

    [StringLength(
        1000,
        ErrorMessage = "Notes cannot exceed 1000 characters.")]
    public string? Notes { get; set; }

    [Required(ErrorMessage = "Contact type is required.")]
    [EnumDataType(
        typeof(ContactType),
        ErrorMessage = "Invalid contact type.")]
    public ContactType ContactType { get; set; }

    [StringLength(
        500,
        ErrorMessage = "Profile photo reference cannot exceed 500 characters.")]
    public string? ProfilePhoto { get; set; }
}