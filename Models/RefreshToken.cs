using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagerServer.Models;

public class RefreshToken
{
    [Key] public int TokenId { get; set; }

    [Required] public int UserId { get; set; }

    [ForeignKey(nameof(UserId))] public User? User { get; set; }

    [Required] public string Token { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsExpired && !IsRevoked;
}