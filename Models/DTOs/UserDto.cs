namespace WarehouseManagerServer.Models.DTOs;

public class UserDto
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? JoinDate { get; set; }
}