using System.IdentityModel.Tokens.Jwt;
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
        var username = dto.Username.Trim();
        var email = dto.Email.Trim();
        var existedUser = await context.Users
            .Where(e => 
                e.Username == username ||
                e.Email == email)
            .FirstOrDefaultAsync();
        if (existedUser != null)
        {
            if (existedUser.Username == username)
                return RegisterEnum.UserAlreadyExists;
            if (existedUser.Email == email)
                return RegisterEnum.EmailAlreadyExists;
        }

        if (email.EndsWith('.')) {
            return RegisterEnum.InvalidEmail;
        }
        try {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
                return RegisterEnum.InvalidEmail;
        }
        catch {
            return RegisterEnum.InvalidEmail;
        }
        
        var user = new User
        {
            Username = username,
            Email = email,
        };
        var passwordHash = passwordHasher.HashPassword(user, password);
        user.PasswordHash = passwordHash;

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

        var username = dto.Username.Trim();
        var user = await context.Users.Where(e => e.Username == username).FirstOrDefaultAsync();
        if (user == null) return null;

        var passwordHash = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        return passwordHash == PasswordVerificationResult.Success ? user : null;
    }
    
    public (string, DateTime) GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Name, user.Username)
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
}