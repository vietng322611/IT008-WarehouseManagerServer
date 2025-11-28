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

    public async Task<Product> AddAsync(ProductDto product)
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
        
        context.Movements.Add(new Movement
        {
            Product = newProduct,
            Quantity = newProduct.Quantity,
            MovementType = MovementTypeEnum.In
        });
        
        await context.SaveChangesAsync();
        
        return newProduct;
    }

    public async Task<Product?> UpdateAsync(ProductDto product)
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
        return oldProduct;
    }

    public async Task UpsertAsync(List<ProductDto> products)
    {
        foreach (var product in products)
        {
            var existing = await UpdateAsync(product);
            if (existing is null)
                await AddAsync(product);
        }
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