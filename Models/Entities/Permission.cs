using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class Permission
{
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }

    [JsonPropertyName("user_permissions")]
    public ICollection<PermissionEnum> UserPermissions { get; set; } = new List<PermissionEnum>();

    [JsonIgnore] public virtual User User { get; set; } = null!;
    [JsonIgnore] public virtual Warehouse Warehouse { get; set; } = null!;
}