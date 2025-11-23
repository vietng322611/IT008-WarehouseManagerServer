using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class ProductRepository(WarehouseContext context) : IProductRepository
{
    public async Task<List<Product>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Products.AsNoTracking()
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .Where(p => p.WarehouseId == warehouseId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByKeyAsync(int productId)
    {
        return await context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Category)
            .Where(p => p.ProductId == productId)
            .FirstOrDefaultAsync();
    }

    // public async Task<List<Product>> FilterAsync(params Expression<Func<Product, bool>>[] filters)
    // {
    //     var query = context.Products.AsQueryable();
    //     query = filters.Aggregate(query, (current, filter) => current.Where(filter));
    //     return await query.ToListAsync();
    // }

    public async Task<Product> AddAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(Product product)
    {
        var oldProduct = await context.Products.FindAsync(product.ProductId);
        if (oldProduct == null) return null;

        context.Entry(oldProduct).CurrentValues.SetValues(product);

        await context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteAsync(int productId)
    {
        var oldProduct = await context.Products.FindAsync(productId);
        if (oldProduct == null) return false;

        context.Products.Remove(oldProduct);
        await context.SaveChangesAsync();
        return true;
    }
}