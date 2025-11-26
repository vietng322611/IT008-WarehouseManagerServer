using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs;

public class UserWarehousesDto
{
    [JsonPropertyName("warehouse")] public required Warehouse Warehouse { get; set; }
    [JsonPropertyName("permissions")] public required ICollection<PermissionEnum> Permissions { get; set; }
}