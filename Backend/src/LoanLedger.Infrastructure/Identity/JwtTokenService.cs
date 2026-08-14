using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoanLedger.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LoanLedger.Infrastructure.Identity;

public class JwtTokenService : IJwtTokenService
{
private readonly JwtSettings _settings;

public JwtTokenService(
    IConfiguration configuration)
{
    _settings = configuration
        .GetSection("JwtSettings")
        .Get<JwtSettings>()!;
}

public string GenerateToken(
    Guid userId,
    string email)
{
    var key =
        new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _settings.SecretKey));

    var credentials =
        new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        // Standard JWT subject claim
        new Claim(
            JwtRegisteredClaimNames.Sub,
            userId.ToString()),

        // ASP.NET Core authenticated user ID
        new Claim(
            ClaimTypes.NameIdentifier,
            userId.ToString()),

        // User email
        new Claim(
            JwtRegisteredClaimNames.Email,
            email),

        // Unique token identifier
        new Claim(
            JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString())
    };

    var token =
        new JwtSecurityToken(
            issuer: _settings.Issuer,

            audience: _settings.Audience,

            claims: claims,

            expires:
                DateTime.UtcNow.AddMinutes(
                    _settings.ExpiryMinutes),

            signingCredentials:
                credentials);

    return new JwtSecurityTokenHandler()
        .WriteToken(token);
}

}
