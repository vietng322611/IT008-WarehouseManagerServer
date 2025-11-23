using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.Entities;

public class Supplier
{
    [JsonPropertyName("supplier_id")] public int SupplierId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("contact_info")] public string? ContactInfo { get; set; }

    [JsonPropertyName("product_count")]
    [NotMapped]
    public int ProductCount { get; set; }

    [JsonIgnore] public virtual Warehouse Warehouse { get; set; } = null!;
    [JsonIgnore] public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}