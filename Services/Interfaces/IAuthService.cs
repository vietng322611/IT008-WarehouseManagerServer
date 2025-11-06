using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterEnum> RegisterUser(RegisterDto dto, string password);
    Task<User?> ValidateUser(LoginDto dto);
    string GenerateAccessToken(User user);
    Task<RefreshToken> GenerateRefreshToken(User user);
    Task<User?> ValidateRefreshToken(RefreshDto dto);
    Task InvalidateRefreshToken(RefreshToken refreshToken);
}