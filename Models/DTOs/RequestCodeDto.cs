using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs;

public class RequestCodeDto
{
    [JsonPropertyName("email")] public string Email { get; set; } = "";
    [JsonPropertyName("type")] public VerificationTypeEnum Type { get; set; }
}