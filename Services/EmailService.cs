using System.Net;
using System.Net.Mail;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class EmailService(
    IConfiguration config,
    IEmailVerificationRepository emailVerfRepository
): IEmailService
{
    public async Task SendEmailAsync(string email, VerificationTypeEnum type)
    {
        var fromAddress = new MailAddress(config["MailService:Address"]!, "WarehouseManager App");
        var toAddress = new MailAddress(email);
        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(
                fromAddress.Address,
                config["MailService:Password"]),
            EnableSsl = true
        };

        var code = await emailVerfRepository.GenerateUniqueCode(email, type);
        using var message = new MailMessage(fromAddress, toAddress);
        var template = await File.ReadAllTextAsync("Resources/Html/EmailTemplate.html");
        message.Subject = "Verification code";
        message.IsBodyHtml = true;
        message.Body = template.Replace("{{CODE}}", code);
        await smtp.SendMailAsync(message);
    }
    
    public async Task<string?> VerifyCode(string code, VerificationTypeEnum type)
    {
        var verificationCode = await emailVerfRepository.GetByCodeAsync(code);
        return verificationCode?.VerificationType != type
            ? null
            : verificationCode.Email;
    }
}