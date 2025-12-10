using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class History
{
    [JsonPropertyName("history_id")] public int HistoryId { get; set; }
    [JsonPropertyName("product_id")] public int ProductId { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("action_type")] public ActionTypeEnum ActionType { get; set; }
    [JsonPropertyName("date")] public DateTime Date { get; set; } = DateTime.UtcNow;

    [JsonIgnore] public virtual Product Product { get; set; } = null!;
}