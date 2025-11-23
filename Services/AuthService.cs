using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Services.Interfaces;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Services;

public class AuthService(
    WarehouseContext context,
    IConfiguration config
) : IAuthService
{
    private readonly PasswordHasher<User> passwordHasher = new();

    public async Task<RegisterEnum> RegisterUser(RegisterDto dto, string password)
    {
        var fullName = dto.FullName.Trim();
        var email = dto.Email.Trim();
        var existedUser = await context.Users
            .Where(e => e.Email == email)
            .FirstOrDefaultAsync();
        if (existedUser != null)
            return RegisterEnum.EmailAlreadyExists;

        if (email.EndsWith('.')) {
            return RegisterEnum.InvalidEmail;
        }
        try {
            var addr = new MailAddress(email);
            if (addr.Address != email)
                return RegisterEnum.InvalidEmail;
        }
        catch {
            return RegisterEnum.InvalidEmail;
        }
        
        var user = new User
        {
            FullName = fullName,
            Email = email,
        };
        user.PasswordHash = passwordHasher.HashPassword(user, password);

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return RegisterEnum.Success;
    }

    public async Task<User?> ValidateUser(LoginDto dto)
    {
        // Clean up old refresh tokens
        context.RefreshTokens.RemoveRange(
            context.RefreshTokens.Where(t => t.ExpiresAt < DateTime.UtcNow)
        );
        await context.SaveChangesAsync();

        var email = dto.Email.Trim();
        var user = await context.Users.Where(e => e.Email == email).FirstOrDefaultAsync();
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
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
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
            ExpiresAt = DateTime.UtcNow.AddDays(7),
        };

        user.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();

        return refreshToken.Token;
    }

    public async Task<User?> ValidateRefreshToken(RefreshDto dto)
    {
        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == dto.RefreshToken));

        return user;
    }

    public async Task InvalidateRefreshToken(RefreshToken refreshToken)
    {
        context.RefreshTokens.Remove(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task SendRecoveryCode(User user)
    {
        var fromAddress = new MailAddress(config["MailService:Address"]!, "WarehouseManager App");
        var toAddress = new MailAddress(user.Email, user.FullName);
        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(
                fromAddress.Address,
                config["MailService:Password"]), EnableSsl = true
        };
        
        var code = await GenerateUniqueCode(user.UserId);
        using var message = new MailMessage(fromAddress, toAddress);
        message.Subject = "Recovery code";
        message.Body = "Recovery code for your WarehouseManager account: " + code;
        await smtp.SendMailAsync(message);
    }

    public async Task<User?> VerifyRecoveryCode(string code)
    {
        var recoveryCode = await context.RecoveryCodes
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Code == code);
        if (recoveryCode == null) return null;
        
        context.RecoveryCodes.Remove(recoveryCode);
        await context.SaveChangesAsync();

        return recoveryCode.User;
    }

    public async Task ChangePassword(User user, string newPassword)
    {
        user.PasswordHash = passwordHasher.HashPassword(user, newPassword);
        await context.SaveChangesAsync();
    }

    private async Task<string> GenerateUniqueCode(int userId)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var bytes = new byte[7];
            RandomNumberGenerator.Fill(bytes);
            var code = new string(bytes.Select(b => chars[b % chars.Length]).ToArray());

            var recovery = new RecoveryCode
            {
                UserId = userId,
                Code = code
            };

            context.RecoveryCodes.Add(recovery);

            try
            {
                await context.SaveChangesAsync();
                return code;
            }
            catch (DbUpdateException) {} // retry
        }

        throw new Exception("Failed to generate a unique recovery code after several attempts.");
    }
}