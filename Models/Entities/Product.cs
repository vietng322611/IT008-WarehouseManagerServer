using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.Entities;

public class Product
{
    [JsonPropertyName("product_id")] public int ProductId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("unit_price")] public decimal UnitPrice { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("supplier_id")] public int SupplierId { get; set; }
    [JsonPropertyName("expiry_date")] public DateTime ExpiryDate { get; set; }

    [JsonIgnore] public virtual Warehouse Warehouse { get; set; } = null!;
    [JsonIgnore] public virtual Supplier Supplier { get; set; } = null!;
    [JsonIgnore] public virtual ICollection<History> Histories { get; set; } = new List<History>();
}