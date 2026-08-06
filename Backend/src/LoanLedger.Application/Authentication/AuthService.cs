using LoanLedger.Application.DTOs;
using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordService _passwordService;

    public AuthService(
        IUserRepository repository,
        IPasswordService passwordService)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    public async Task<RegisterUserResponse> RegisterAsync(RegisterUserRequest request)
	{
		if (request.Password != request.ConfirmPassword)
		{
			return new RegisterUserResponse
			{
				Success = false,
				Message = "Passwords do not match."
			};
		}

		var existingEmail = await _repository.GetByEmailAsync(request.Email);

		if (existingEmail != null)
		{
			return new RegisterUserResponse
			{
				Success = false,
				Message = "Email already exists."
			};
		}

		var existingPhone = await _repository.GetByPhoneAsync(request.PhoneNumber);

		if (existingPhone != null)
		{
			return new RegisterUserResponse
			{
				Success = false,
				Message = "Phone number already exists."
			};
		}

		var user = new User
		{
			Id = Guid.NewGuid(),
			FullName = request.FullName,
			Email = request.Email,
			PhoneNumber = request.PhoneNumber,
			PasswordHash = _passwordService.HashPassword(request.Password),
			IsActive = true,
			IsEmailVerified = false,
			IsPhoneVerified = false
		};

		await _repository.AddAsync(user);

		await _repository.SaveChangesAsync();

		return new RegisterUserResponse
		{
			Success = true,
			Message = "Registration successful.",
			UserId = user.Id
		};
	}
	public async Task<LoginResponse> LoginAsync(LoginRequest request)
	{
		throw new NotImplementedException();
	}
}