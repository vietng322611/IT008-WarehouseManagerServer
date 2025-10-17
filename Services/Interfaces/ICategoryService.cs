using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByKeyAsync(int categoryId);
    Task<Category> AddAsync(Category category);
    Task<Category?> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int categoryId);
}