using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs;

public class WarehouseUsersDto
{
    [JsonPropertyName("user_id")] public required int UserId { get; set; }
    [JsonPropertyName("fullName")] public required string FullName { get; set; }
    [JsonPropertyName("permissions")] public required IEnumerable<PermissionEnum> Permissions { get; set; }
}