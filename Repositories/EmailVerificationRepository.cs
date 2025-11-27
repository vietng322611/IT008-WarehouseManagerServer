using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class EmailVerificationRepository(WarehouseContext context): IEmailVerificationRepository
{
    public async Task<EmailVerification?> GetByCodeAsync(string code)
    {
        return await context.EmailVerifications
            .FirstOrDefaultAsync(x => x.Code == code);
    }

    public async Task RemoveAsync(EmailVerification emailVerification)
    {
        context.EmailVerifications.Remove(emailVerification);
        await context.SaveChangesAsync();
    }

    public async Task ClearOutdatedAsync()
    {
        context.EmailVerifications.RemoveRange(
            context.EmailVerifications.Where(t => t.ExpiresAt < DateTime.UtcNow)
        );
        await context.SaveChangesAsync();
    }

    public async Task<string> GenerateUniqueCode(string email, VerificationTypeEnum type)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        for (var attempt = 0; attempt < 5; attempt++)
        {
            var bytes = new byte[7];
            RandomNumberGenerator.Fill(bytes);
            var code = new string(bytes.Select(b => chars[b % chars.Length]).ToArray());

            var recovery = new EmailVerification
            {
                Code = code,
                Email = email,
                VerificationType = type
            };

            context.EmailVerifications.Add(recovery);

            try
            {
                await context.SaveChangesAsync();
                return code;
            }
            catch (DbUpdateException)
            {
            } // retry
        }

        throw new Exception("Failed to generate a unique recovery code after several attempts.");
    }
}