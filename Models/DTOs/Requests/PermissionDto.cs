using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class PermissionDto
{
    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("user_permissions")]
    public List<PermissionEnum> UserPermissions { get; set; } = [];
}