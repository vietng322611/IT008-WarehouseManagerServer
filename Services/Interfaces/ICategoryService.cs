using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetByWarehouseAsync(int warehouseId);
    Task<Category?> GetByKeyAsync(int categoryId);
    Task<Category> AddAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int categoryId);
}