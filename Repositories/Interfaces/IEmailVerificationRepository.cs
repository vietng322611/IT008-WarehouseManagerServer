using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IEmailVerificationRepository
{
    Task<EmailVerification?> GetByCodeAsync(string code);
    Task RemoveAsync(EmailVerification emailVerification);
    Task ClearOutdatedAsync();
    Task<string> GenerateUniqueCode(string email, VerificationTypeEnum type);
}