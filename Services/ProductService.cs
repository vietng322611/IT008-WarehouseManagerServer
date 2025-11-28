using WarehouseManagerServer.Models.DTOs.Requests;
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

    public Task<Product> AddAsync(ProductDto product)
        => productRepository.AddAsync(product);

    public Task<Product?> UpdateAsync(ProductDto product)
        => productRepository.UpdateAsync(product);

    public Task UpsertAsync(List<ProductDto> products)
        => productRepository.UpsertAsync(products);

    public Task<bool> DeleteAsync(int productId)
        => productRepository.DeleteAsync(productId);
}