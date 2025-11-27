using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagerServer.Models.Entities;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("email")] public required string Email { get; set; }
    [JsonPropertyName("full_name")] public required string FullName { get; set; }
    [JsonPropertyName("join_date")] public DateTime JoinDate { get; set; } = DateTime.Now;

    [JsonIgnore] public string PasswordHash { get; set; } = null!;
    [JsonIgnore] public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    [JsonIgnore] public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    [JsonIgnore] public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}