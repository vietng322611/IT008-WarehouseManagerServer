using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.DTOs;

public class MovementDto
{
    public int MovementId { get; set; }

    public string ProductName { get; set; } = null!;

    public int Quantity { get; set; }

    public MovementTypeEnum MovementTypeEnum { get; set; }

    public DateTime? Date { get; set; }
}