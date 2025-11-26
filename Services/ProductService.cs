using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public Task<List<Product>> GetByWarehouseAsync(int warehouseId)
        => productRepository.GetByWarehouseAsync(warehouseId);

    public Task<Product?> GetByKeyAsync(int productId)
        => productRepository.GetByKeyAsync(productId);

    // public Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters)
    //     => productRepository.FilterAsync(filters);

    public Task<Product> AddAsync(Product product)
        => productRepository.AddAsync(product);

    public Task<Product?> UpdateAsync(Product product)
        => productRepository.UpdateAsync(product);

    public Task UpsertAsync(List<Product> products)
        => productRepository.UpsertAsync(products);

    public Task<bool> DeleteAsync(int productId)
        => productRepository.DeleteAsync(productId);
}