namespace WarehouseManagerServer.Models.Entities;

public class Category
{
    public int CategoryId { get; set; }

    public int WarehouseId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Warehouse Warehouse { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}