using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class RegisterDto
{
    [JsonPropertyName("full_name")] public string FullName { get; set; } = "";
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("password")] public string Password { get; set; } = "";
}