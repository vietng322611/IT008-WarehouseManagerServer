using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetByWarehouseAsync(int warehouseId);
    Task<Category?> GetByKeyAsync(int categoryId);
    Task<Category> AddAsync(CategoryDto category);
    Task<Category?> UpdateAsync(CategoryDto category);
    Task UpsertAsync(List<CategoryDto> categories);
    Task<bool> DeleteAsync(int categoryId);
}