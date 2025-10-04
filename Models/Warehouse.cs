namespace WarehouseManagerServer.Models;

public class Warehouse
{
    public int WarehouseId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Product> Products { get; set; }
    
    public ICollection<User> Users { get; set; } = new List<User>();
}