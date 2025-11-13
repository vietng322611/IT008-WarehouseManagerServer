namespace WarehouseManagerServer.Models.Entities;

public class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int WarehouseId { get; set; }

    public int? SupplierId { get; set; }

    public int? CategoryId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }
    
    public DateTime ExpiryDate { get; set; }

    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual Supplier? Supplier { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Movement> Movements { get; set; } = new List<Movement>();
}