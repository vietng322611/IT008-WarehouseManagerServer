namespace WarehouseManagerServer.Models.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }

    public string? SupplierName { get; set; }
    public string? CategoryName { get; set; }
}