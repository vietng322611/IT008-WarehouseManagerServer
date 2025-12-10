using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
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

    public async Task<Product> AddAsync(ProductDto product, int userId)
    {
        var newProduct = new Product
        {
            Name = product.Name,
            Quantity = product.Quantity,
            UnitPrice = product.UnitPrice,
            WarehouseId = product.WarehouseId,
            SupplierId = product.SupplierId,
            CategoryId = product.CategoryId,
            ExpiryDate = product.ExpiryDate
        };
        context.Products.Add(newProduct);
        
        context.Histories.Add(new History
        {
            Product = newProduct,
            Quantity = newProduct.Quantity,
            ActionType = ActionTypeEnum.In
        });
        
        await context.SaveChangesAsync();
        
        return newProduct;
    }

    public async Task<Product?> UpdateAsync(ProductDto product, int userId, ActionTypeEnum actionType)
    {
        var oldProduct = await context.Products.FindAsync(product.ProductId);
        if (oldProduct == null) return null;

        oldProduct.Name = product.Name;
        oldProduct.Quantity = product.Quantity;
        oldProduct.UnitPrice = product.UnitPrice;
        oldProduct.SupplierId = product.SupplierId;
        oldProduct.CategoryId = product.CategoryId;
        oldProduct.ExpiryDate = product.ExpiryDate;

        await context.SaveChangesAsync();
        await LogAsync(product, userId, actionType);
        return oldProduct;
    }

    public async Task<bool> DeleteAsync(int productId, int userId)
    {
        var oldProduct = await context.Products.FindAsync(productId);
        if (oldProduct == null) return false;

        context.Products.Remove(oldProduct);
        await context.SaveChangesAsync();
        return true;
    }

    private async Task LogAsync(ProductDto product, int userId, ActionTypeEnum actionType)
    {
        context.Histories.Add(new History
        {
            ProductId = product.ProductId,
            UserId = userId,
            Quantity = product.Quantity,
            ActionType = actionType,
            Date = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
    }
}