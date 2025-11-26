using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs;

public class WarehouseUsersDto
{
    [JsonPropertyName("user")] public required User User { get; set; }
    [JsonPropertyName("permission")] public required ICollection<PermissionEnum> Permissions { get; set; }
}