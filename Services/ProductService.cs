using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
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

    public Task<Product> AddAsync(ProductDto product, int userId)
        => productRepository.AddAsync(product, userId);

    public Task<Product?> UpdateAsync(ProductDto product, int userId, ActionTypeEnum actionType)
        => productRepository.UpdateAsync(product, userId, actionType);

    public Task<bool> DeleteAsync(int productId, int userId)
        => productRepository.DeleteAsync(productId, userId);
}