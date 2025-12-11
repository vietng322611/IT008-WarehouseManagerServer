using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetByWarehouseAsync(int warehouseId);

    Task<Product?> GetByKeyAsync(int productId);

    // Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters);
    Task<Product> AddAsync(ProductDto product, int userId);
    Task<List<Product>> UpdateQuantityAsync(List<ProductDto> products, int userId, ActionTypeEnum actionType);
    Task<List<Product>> UpdateMetaAsync(List<ProductDto> products, int userId);
    Task<bool> DeleteAsync(int productId, int userId);
}