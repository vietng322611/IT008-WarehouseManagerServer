using Resend;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class EmailService(
    IConfiguration config,
    IResend resend,
    IEmailVerificationRepository emailVerfRepository
): IEmailService
{
    public async Task SendEmailAsync(string email, VerificationTypeEnum type)
    {
        var code = await emailVerfRepository.GenerateUniqueCode(email, type);
        
        var template = await File.ReadAllTextAsync("Resources/Html/EmailTemplate.html");
        var htmlBody = template.Replace("{{CODE}}", code);
        
        var emailMessage = new EmailMessage
        {
            From = config["Resend:FromEmail"]!,
            To = email,
            Subject = "Verification code",
            HtmlBody = htmlBody
        };
        
        var response = await resend.EmailSendAsync(emailMessage);

        if (!response.Success)
        {
            throw new Exception(
                $"Resend failed: {response.Exception?.Message}");
        }
    }
    
    public async Task<string?> VerifyCode(string code, VerificationTypeEnum type)
    {
        var verificationCode = await emailVerfRepository.GetByCodeAsync(code);
        return verificationCode?.VerificationType != type
            ? null
            : verificationCode.Email;
    }
}