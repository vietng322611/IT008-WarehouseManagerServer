using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetByWarehouseAsync(int warehouseId);

    Task<Product?> GetByKeyAsync(int productId);

    // Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters);
    Task<Product> AddAsync(Product product);
    Task<Product?> UpdateAsync(Product products);
    Task UpsertAsync(List<Product> products);
    Task<bool> DeleteAsync(int productId);
}