using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetByWarehouseAsync(int warehouseId);

    Task<Product?> GetByKeyAsync(int productId);

    // Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters);
    Task<Product> AddAsync(ProductDto product);
    Task<Product?> UpdateAsync(ProductDto products);
    Task UpsertAsync(List<ProductDto> products);
    Task<bool> DeleteAsync(int productId);
}