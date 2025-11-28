using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs.Requests;

public class UserDto
{
    [JsonPropertyName("full_name")] public  string FullName { get; set; }
}