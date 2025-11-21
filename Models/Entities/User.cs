using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagerServer.Models.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [JsonPropertyName(("user_id"))]
    public int UserId { get; set; }
    [JsonPropertyName(("username"))]
    public string Username { get; set; } = null!;
    [JsonPropertyName(("email"))]
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    [JsonPropertyName(("join_date"))]
    public DateTime? JoinDate { get; set; }

    public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}