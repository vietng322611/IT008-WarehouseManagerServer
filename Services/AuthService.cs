using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Services;

public class AuthService(
    IConfiguration config,
    IUserRepository userRepository,
    IEmailService emailService
) : IAuthService
{
    private readonly PasswordHasher<User> passwordHasher = new();

    public async Task<RegisterEnum> RegisterUser(string fullName, string email, string password)
    {
        email = email.Trim();
        var existedUser = await userRepository
            .GetByUniqueAsync(user => user.Email == email);
        if (existedUser != null)
            return RegisterEnum.EmailAlreadyExists;

        if (email.EndsWith('.')) return RegisterEnum.InvalidEmail;
        try
        {
            var addr = new MailAddress(email);
            if (addr.Address != email)
                return RegisterEnum.InvalidEmail;
        }
        catch
        {
            return RegisterEnum.InvalidEmail;
        }

        var user = new User
        {
            FullName = fullName.Trim(),
            Email = email
        };
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        await userRepository.AddAsync(user);

        return RegisterEnum.Success;
    }

    public async Task<User?> ValidateUser(LoginDto dto)
    {
        // Clean up old refresh tokens
        await userRepository.ClearOutdatedAsync();

        var email = dto.Email.Trim();
        var user = await userRepository
            .GetByUniqueAsync(user => user.Email == email);
        if (user == null) return null;

        var passwordHash = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        return passwordHash == PasswordVerificationResult.Success ? user : null;
    }

    public (string, DateTime) GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:AccessTokenExpirationMinutes"]!));

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: credentials
        );

        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }

    public async Task<string> GenerateRefreshToken(User user)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await userRepository.AddTokenAsync(user, refreshToken);

        return refreshToken.Token;
    }

    public async Task<User?> ValidateRefreshToken(RefreshDto dto)
        => await userRepository.GetUserFromToken(dto.RefreshToken);

    public async Task InvalidateRefreshToken(RefreshToken refreshToken)
        => await userRepository.InvalidateRefreshToken(refreshToken);

    public async Task SendVerificationCode(User user, VerificationTypeEnum type)
        => await emailService.SendEmailAsync(user.Email, type);

    public async Task<User?> VerifyRecoveryCode(string code)
    {
        var email = await emailService.VerifyCode(code, VerificationTypeEnum.Recovery);
        if (email == null) return null;

        var user = await userRepository.GetByUniqueAsync(user => user.Email == email);
        if (user != null) return user;
        
        return new User // use null to identify register/recovery and reset
        {
            FullName = "",
            Email = email
        };
    }

    public async Task ChangePassword(User user, string newPassword)
        => await userRepository.ChangePassword(user, passwordHasher.HashPassword(user, newPassword));
}