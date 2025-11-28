using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetByWarehouseAsync(int warehouseId);
    Task<Category?> GetByKeyAsync(int categoryId);
    Task<Category> AddAsync(CategoryDto category);
    Task<Category?> UpdateAsync(CategoryDto category);
    Task UpsertAsync(List<CategoryDto> categories);
    Task<bool> DeleteAsync(int categoryId);
}