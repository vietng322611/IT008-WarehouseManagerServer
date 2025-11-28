using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetByWarehouseAsync(int warehouseId);

    Task<Product?> GetByKeyAsync(int productId);

    // Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters);
    Task<Product> AddAsync(ProductDto product);
    Task<Product?> UpdateAsync(ProductDto product);
    Task UpsertAsync(List<ProductDto> products);
    Task<bool> DeleteAsync(int productId);
}