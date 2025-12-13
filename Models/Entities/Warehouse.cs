using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.Entities;

public class Warehouse
{
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("create_date")] public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    [JsonIgnore] public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
    [JsonIgnore] public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    [JsonIgnore] public virtual ICollection<User> Users { get; set; } = new List<User>();
    [JsonIgnore] public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}