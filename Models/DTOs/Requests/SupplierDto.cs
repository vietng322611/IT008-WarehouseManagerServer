using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class SupplierDto
{
    [JsonPropertyName("supplier_id")] public int SupplierId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }
    [JsonPropertyName("contact_info")] public string? ContactInfo { get; set; }
}