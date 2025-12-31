using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class History
{
    [JsonPropertyName("history_id")] public int HistoryId { get; set; }
    [JsonPropertyName("warehouse_id")] public int WarehouseId { get; set; }
    [JsonPropertyName("product_name")] public string ProductName { get; set; }
    [JsonPropertyName("supplier_name")] public string SupplierName { get; set; }
    [JsonPropertyName("user_full_name")] public string UserFullName { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("action_type")] public ActionTypeEnum ActionType { get; set; }
    [JsonPropertyName("date")] public DateTime Date { get; set; } = DateTime.UtcNow;
    
    [JsonIgnore] public virtual Warehouse Warehouse { get; set; } = null!;
}