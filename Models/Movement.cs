using WarehouseManagerServer.Enums;

namespace WarehouseManagerServer.Models;

public class Movement
{
    public int MovementId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public MovementTypeEnum MovementTypeEnum { get; set; }

    public DateTime? Date { get; set; }

    public virtual Product Product { get; set; } = null!;
}