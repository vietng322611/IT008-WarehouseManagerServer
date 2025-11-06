using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public Task<List<Category>> GetByWarehouseAsync(int warehouseId)
        => categoryRepository.GetByWarehouseAsync(warehouseId);

    public Task<Category?> GetByKeyAsync(int categoryId)
        => categoryRepository.GetByKeyAsync(categoryId);

    public Task<Category> AddAsync(Category category)
        => categoryRepository.AddAsync(category);

    public Task<Category?> UpdateAsync(Category category)
        => categoryRepository.UpdateAsync(category);

    public Task<bool> DeleteAsync(int categoryId)
        => categoryRepository.DeleteAsync(categoryId);
}