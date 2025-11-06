using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Models.Entities;

public class Movement
{
    public int MovementId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public MovementTypeEnum MovementType { get; set; }

    public DateTime? Date { get; set; }

    public virtual Product Product { get; set; } = null!;
}