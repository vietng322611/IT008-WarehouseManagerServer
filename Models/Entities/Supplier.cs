namespace WarehouseManagerServer.Models.Entities;

public class Supplier
{
    public int SupplierId { get; set; }

    public int WarehouseId { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}