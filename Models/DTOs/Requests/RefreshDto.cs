using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class RefreshDto
{
    [JsonPropertyName("refresh_token")] public string RefreshToken { get; set; } = "";
}