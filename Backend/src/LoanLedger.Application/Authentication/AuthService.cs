using LoanLedger.Application.DTOs;
using LoanLedger.Application.Interfaces;
using LoanLedger.Domain.Entities;

namespace LoanLedger.Application.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordService _passwordService;
	private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
		IUserRepository repository,
		IPasswordService passwordService,
		IJwtTokenService jwtTokenService)
	{
		_repository = repository;
		_passwordService = passwordService;
		_jwtTokenService = jwtTokenService;
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
		var user = await _repository.GetByEmailOrPhoneAsync(request.EmailOrPhone);

		if (user == null)
		{
			return new LoginResponse
			{
				Success = false,
				Message = "Invalid email/phone or password."
			};
		}

		if (!user.IsActive)
		{
			return new LoginResponse
			{
				Success = false,
				Message = "Account has been disabled."
			};
		}

		var validPassword =
			_passwordService.VerifyPassword(
				user.PasswordHash,
				request.Password);

		if (!validPassword)
		{
			return new LoginResponse
			{
				Success = false,
				Message = "Invalid email/phone or password."
			};
		}

		var token =
			_jwtTokenService.GenerateToken(
				user.Id,
				user.Email);

		user.LastLoginAt = DateTime.UtcNow;

		await _repository.SaveChangesAsync();

		return new LoginResponse
		{
			Success = true,
			Message = "Login successful.",
			Token = token,
			UserId = user.Id,
			FullName = user.FullName
		};
	}
}