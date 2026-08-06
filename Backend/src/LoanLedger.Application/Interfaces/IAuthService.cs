using LoanLedger.Application.DTOs;

namespace LoanLedger.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request);

    Task<LoginResponse> LoginAsync(LoginRequest request);
}