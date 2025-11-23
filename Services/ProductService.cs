using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public Task<List<Product>> GetByWarehouseAsync(int warehouseId)
    {
        return productRepository.GetByWarehouseAsync(warehouseId);
    }

    public Task<Product?> GetByKeyAsync(int productId)
    {
        return productRepository.GetByKeyAsync(productId);
    }

    // public Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters)
    //     => productRepository.FilterAsync(filters);

    public Task<Product> AddAsync(Product product)
    {
        return productRepository.AddAsync(product);
    }

    public Task<Product?> UpdateAsync(Product product)
    {
        return productRepository.UpdateAsync(product);
    }

    public Task<bool> DeleteAsync(int productId)
    {
        return productRepository.DeleteAsync(productId);
    }
}