using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class SupplierStat
{
    [JsonPropertyName("name")] public string Name { get; set; } = "";
    [JsonPropertyName("amount")] public int Count { get; set; }
}

public class StatisticDto
{
    [JsonPropertyName("supplier_stats")] public List<SupplierStat> SupplierStats { get; set; } = [];
    [JsonPropertyName("expired")] public List<int> Expired { get; set; } = [];
    [JsonPropertyName("sale")] public List<int> Sale { get; set; } = [];
    [JsonPropertyName("import")] public int Import { get; set; }
    [JsonPropertyName("export")] public int Export { get; set; }
}