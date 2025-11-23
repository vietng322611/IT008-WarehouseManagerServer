using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagerServer.Models.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public RecoveryCode? RecoveryCode;
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; } = null!;
    [JsonPropertyName("full_name")] public string FullName { get; set; } = null!;
    [JsonPropertyName("join_date")] public DateTime? JoinDate { get; set; }

    [JsonIgnore] public string PasswordHash { get; set; } = null!;
    [JsonIgnore] public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    [JsonIgnore] public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    [JsonIgnore] public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}