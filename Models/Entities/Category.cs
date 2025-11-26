using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.Entities;

public class Category
{
    [JsonPropertyName("category_id")] public int CategoryId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("name")] public required string Name { get; set; }

    [JsonIgnore] public virtual Warehouse Warehouse { get; set; } = null!;
    [JsonIgnore] public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}