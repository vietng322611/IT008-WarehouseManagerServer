using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterEnum> RegisterUser(string fullName, string email, string password);
    Task<User?> ValidateUser(LoginDto dto);
    (string, DateTime) GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    Task<User?> ValidateRefreshToken(RefreshDto dto);
    Task InvalidateRefreshToken(RefreshToken refreshToken);
    Task SendVerificationCode(string email, VerificationTypeEnum type);
    Task<User?> VerifyCode(string code, VerificationTypeEnum type);
    Task ChangePassword(User user, string newPassword);
}