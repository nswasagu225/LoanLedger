using System.ComponentModel.DataAnnotations;

namespace LoanLedger.Application.DTOs;

public class LoginRequest
{
    [Required]
    public string EmailOrPhone { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}