using System.Linq.Expressions;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetByWarehouseAsync(int warehouseId);

    Task<Product?> GetByKeyAsync(int productId);

    // Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters);
    Task<Product> AddAsync(Product product);
    Task<Product?> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int productId);
}