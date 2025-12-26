using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class WarehouseUsersDto
{
    [JsonPropertyName("user_id")] public required int UserId { get; set; }
    [JsonPropertyName("full_name")] public required string FullName { get; set; }
    [JsonPropertyName("user_permissions")] public required ICollection<PermissionEnum> Permissions { get; set; }
}