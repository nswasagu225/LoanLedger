namespace LoanLedger.Application.DTOs.Profile;

public class ProfileDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? ProfileImage { get; set; }

    public bool IsActive { get; set; }

    public bool IsEmailVerified { get; set; }

    public bool IsPhoneVerified { get; set; }

    public DateTime? LastLoginAt { get; set; }
}