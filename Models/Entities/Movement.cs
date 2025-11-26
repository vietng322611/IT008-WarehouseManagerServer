using System.Text.Json.Serialization;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class Movement
{
    [JsonPropertyName("movement_id")] public int MovementId { get; set; }
    [JsonPropertyName("product_id")] public int ProductId { get; set; }
    [JsonPropertyName("quantity")] public int Quantity { get; set; }
    [JsonPropertyName("movement_type")] public MovementTypeEnum MovementType { get; set; }
    [JsonPropertyName("date")] public DateTime Date { get; set; } = DateTime.Now;

    [JsonIgnore] public virtual Product Product { get; set; } = null!;
}