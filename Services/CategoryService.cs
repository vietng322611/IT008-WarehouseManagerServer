using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class CategoryService(ICategoryRepository categoryRepository): ICategoryService
{
    public Task<IEnumerable<Category>> GetAllAsync() 
        => categoryRepository.GetAllAsync();
    public Task<Category?> GetByKeyAsync(int categoryId) 
        => categoryRepository.GetByKeyAsync(categoryId);
    public Task<Category> AddAsync(Category category) 
        => categoryRepository.AddAsync(category);
    public Task<Category?> UpdateAsync(Category category) 
        => categoryRepository.UpdateAsync(category);
    public Task<bool> DeleteAsync(int categoryId) 
        => categoryRepository.DeleteAsync(categoryId);
}