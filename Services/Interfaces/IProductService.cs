using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetByWarehouseAsync(int warehouseId);

    Task<Product?> GetByKeyAsync(int productId);

    // Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters);
    Task<Product> AddAsync(ProductDto product, int userId);
    Task<Product?> UpdateAsync(ProductDto product, int userId, ActionTypeEnum actionType);
    Task<bool> DeleteAsync(int productId, int userId);
}