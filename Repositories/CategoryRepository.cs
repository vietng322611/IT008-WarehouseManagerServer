using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
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

    public async Task<Category> AddAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(Category category)
    {
        var oldCategory = await GetByKeyAsync(category.CategoryId);
        if (oldCategory == null) return null;

        context.Entry(oldCategory).CurrentValues.SetValues(category);
        await context.SaveChangesAsync();
        return category;
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