using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterEnum> RegisterUser(RegisterDto dto, string password);
    Task<User?> ValidateUser(LoginDto dto);
    (string, DateTime) GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    Task<User?> ValidateRefreshToken(RefreshDto dto);
    Task InvalidateRefreshToken(RefreshToken refreshToken);
    Task SendRecoveryCode(User user);
    Task<User?> VerifyRecoveryCode(string code);
    Task ChangePassword(User user, string newPassword);
}