using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class ResetPasswordDto
{
    [JsonPropertyName("code")] public string Code { get; set; } = "";
    [JsonPropertyName("new_password")] public string NewPassword { get; set; } = "";
}