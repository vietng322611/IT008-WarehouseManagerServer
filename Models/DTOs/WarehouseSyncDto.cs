using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Models.DTOs;

public class WarehouseSyncDto
{
    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Supplier> Suppliers { get; set; } = new();
    public List<Movement> Movements { get; set; } = new();
}