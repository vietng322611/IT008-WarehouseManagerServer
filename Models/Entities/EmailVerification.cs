using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

[Index(nameof(Code), IsUnique = true)] // Multiple code allowed so email is not unique
public class EmailVerification
{
    public int CodeId { get; set; }
    public required string Email { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow + TimeSpan.FromMinutes(10);
    public required VerificationTypeEnum VerificationType { get; set; }
}