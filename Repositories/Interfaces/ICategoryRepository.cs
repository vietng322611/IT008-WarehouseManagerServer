using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetByWarehouseAsync(int warehouseId);
    Task<Category?> GetByKeyAsync(int categoryId);
    Task<Category> AddAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int categoryId);
}