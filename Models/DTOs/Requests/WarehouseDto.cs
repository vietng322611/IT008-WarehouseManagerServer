using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class WarehouseDto
{
    [JsonPropertyName("name")] public string Name { get; set; }
}