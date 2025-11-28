using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class CategoryRepository(WarehouseContext context) : ICategoryRepository
{
    public async Task<List<Category>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Categories
            .Where(c => c.WarehouseId == warehouseId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetByKeyAsync(int categoryId)
    {
        return await context.Categories.FindAsync(categoryId);
    }

    public async Task<Category> AddAsync(CategoryDto category)
    {
        var newCategory = context.Categories.Add(new Category
        {
            Name = category.Name,
            WarehouseId = category.WarehouseId
        });
        await context.SaveChangesAsync();
        return newCategory.Entity;
    }

    public async Task<Category?> UpdateAsync(CategoryDto category)
    {
        var oldCategory = await GetByKeyAsync(category.CategoryId);
        if (oldCategory == null) return null;

        oldCategory.Name = category.Name;
        
        await context.SaveChangesAsync();
        return oldCategory;
    }
    
    public async Task UpsertAsync(List<CategoryDto> categories)
    {
        foreach (var category in categories)
        {
            var existing = await UpdateAsync(category);
            if (existing is null)
                await AddAsync(category);
        }
    }

    public async Task<bool> DeleteAsync(int categoryId)
    {
        var oldCategory = await GetByKeyAsync(categoryId);
        if (oldCategory == null) return false;

        context.Categories.Remove(oldCategory);
        await context.SaveChangesAsync();
        return true;
    }
}