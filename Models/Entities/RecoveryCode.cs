namespace WarehouseManagerServer.Models.Entities;

public class RecoveryCode
{
    public int CodeId { get; set; }
    public int UserId { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
}