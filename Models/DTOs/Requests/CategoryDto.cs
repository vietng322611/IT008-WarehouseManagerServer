using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class CategoryDto
{
    [JsonPropertyName("category_id")] public int CategoryId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
}