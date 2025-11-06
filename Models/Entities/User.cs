using Microsoft.EntityFrameworkCore;

namespace WarehouseManagerServer.Models.Entities;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? JoinDate { get; set; }

    public ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}