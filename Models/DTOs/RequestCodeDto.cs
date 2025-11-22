using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class RequestCodeDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";
}