using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class LoginDto
{
    [JsonPropertyName(("username"))]
    public string Username { get; set; } = "";
    [JsonPropertyName(("password"))]
    public string Password { get; set; } = "";
}