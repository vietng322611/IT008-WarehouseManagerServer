using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class PermissionUpdDto
{
    [JsonPropertyName("user_id")] public int UserId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }

    [JsonPropertyName("user_permissions")]
    public ICollection<PermissionEnum> UserPermissions { get; set; } = new List<PermissionEnum>();
}   