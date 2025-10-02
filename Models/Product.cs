namespace WarehouseManagerServer.Models;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Sku { get; set; }

    public int WarehouseId { get; set; }

    public int? CategoryId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Warehouse? Warehouse { get; set; }

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
}