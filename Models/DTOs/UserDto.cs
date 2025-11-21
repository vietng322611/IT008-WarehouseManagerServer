using System.Text.Json.Serialization;

namespace WarehouseManagerServer.Models.DTOs;

public class UserDto
{
    [JsonPropertyName(("user_id"))]
    public int UserId { get; set; }
    [JsonPropertyName(("username"))]
    public string Username { get; set; } = null!;
    [JsonPropertyName(("email"))]
    public string Email { get; set; } = null!;
    [JsonPropertyName(("join_date"))]
    public DateTime? JoinDate { get; set; }
}