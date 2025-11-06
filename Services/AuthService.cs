using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class AuthService(
    WarehouseContext context,
    IConfiguration config
) : IAuthService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<RegisterEnum> RegisterUser(RegisterDto dto, string password)
    {
        var existedUser = await context.Users.Where(e => e.Username == dto.Username).FirstOrDefaultAsync();
        if (existedUser != null) return RegisterEnum.UserAlreadyExists;

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
        };
        var passwordHash = _passwordHasher.HashPassword(user, password);
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

        var user = await context.Users.Where(e => e.Username == dto.Username).FirstOrDefaultAsync();
        if (user == null) return null;

        var passwordHash = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        return passwordHash == PasswordVerificationResult.Success ? user : null;
    }


    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:AccessTokenExpirationMinutes"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<RefreshToken> GenerateRefreshToken(User user)
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

        return refreshToken;
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