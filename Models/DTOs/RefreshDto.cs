using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class RefreshDto
{
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = "";
}