using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class ProductDto
{
    [JsonPropertyName("product_id")] public int ProductId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("supplier_id")] public int? SupplierId { get; set; }
    [JsonPropertyName("category_id")] public int? CategoryId { get; set; }
    [JsonPropertyName("unit_price")] public decimal UnitPrice { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("expiry_date")] public DateTime ExpiryDate { get; set; }
}