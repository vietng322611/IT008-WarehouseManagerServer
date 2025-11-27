using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string email, VerificationTypeEnum type);
    Task<string?> VerifyCode(string code, VerificationTypeEnum type);
}