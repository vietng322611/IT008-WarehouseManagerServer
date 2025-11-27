using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class ChangePasswordDto
{
    [JsonPropertyName("code")] public string Code { get; set; } = "";
    [JsonPropertyName("old_password")] public string OldPassword { get; set; } = "";
    [JsonPropertyName("new_password")] public string NewPassword { get; set; } = "";
}