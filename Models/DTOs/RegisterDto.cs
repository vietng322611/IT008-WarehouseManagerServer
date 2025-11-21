using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class RegisterDto
{
    [JsonPropertyName(("username"))]
    public string Username { get; set; } = "";
    [JsonPropertyName(("email"))]
    public string Email { get; set; } = "";
    [JsonPropertyName(("password"))]
    public string Password { get; set; } = "";
}