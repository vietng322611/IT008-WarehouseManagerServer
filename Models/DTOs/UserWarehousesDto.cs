using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs;

public class UserWarehousesDto
{
    [JsonPropertyName("warehouse_id")] public required int WarehouseId { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("create_date")] public DateTime CreateDate { get; set; }
    [JsonPropertyName("permissions")] public required IEnumerable<PermissionEnum> Permissions { get; set; }
}