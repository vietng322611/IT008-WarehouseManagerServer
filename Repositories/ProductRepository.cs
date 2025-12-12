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
            .Where(p => p.WarehouseId == warehouseId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product?> GetByKeyAsync(int productId)
    {
        return await context.Products.AsNoTracking()
            .Include(p => p.Supplier)
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
            ExpiryDate = product.ExpiryDate
        };
        context.Products.Add(newProduct);
        
        Log(newProduct, userId, ActionTypeEnum.In);
        
        await context.SaveChangesAsync();
        return await context.Products.AsNoTracking()
            .Include(p => p.Supplier)
            .FirstAsync(p => p.ProductId == newProduct.ProductId);
    }

    public async Task<List<Product>> UpdateQuantityAsync(List<ProductDto> products, int userId, ActionTypeEnum actionType)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var updatedIds = new List<int>();
            
            foreach (var dto in products)
            {
                var oldProduct = await context.Products.FindAsync(dto.ProductId);
                if (oldProduct == null) continue;

                oldProduct.Quantity = dto.Quantity;

                updatedIds.Add(oldProduct.ProductId);
                Log(oldProduct, userId, actionType);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await context.Products.AsNoTracking()
                .Include(p => p.Supplier)
                .Where(p => updatedIds.Contains(p.ProductId))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<List<Product>> UpdateMetaAsync(List<ProductDto> products, int userId)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var updatedIds = new List<int>();
            
            foreach (var dto in products)
            {
                var oldProduct = await context.Products.FindAsync(dto.ProductId);
                if (oldProduct == null) continue;

                oldProduct.Name = dto.Name;
                oldProduct.UnitPrice = dto.UnitPrice;
                oldProduct.SupplierId = dto.SupplierId;
                oldProduct.ExpiryDate = dto.ExpiryDate;

                updatedIds.Add(oldProduct.ProductId);
                Log(oldProduct, userId, ActionTypeEnum.Modify);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await context.Products.AsNoTracking()
                .Include(p => p.Supplier)
                .Where(p => updatedIds.Contains(p.ProductId))
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int productId, int userId)
    {
        var oldProduct = await context.Products.FindAsync(productId);
        if (oldProduct == null) return false;

        context.Products.Remove(oldProduct);
        Log(oldProduct, userId, ActionTypeEnum.Remove);
        await context.SaveChangesAsync();
        return true;
    }

    private void Log(Product product, int userId, ActionTypeEnum actionType)
    {
        context.Histories.Add(new History
        {
            ProductId = product.ProductId,
            UserId = userId,
            Quantity = product.Quantity,
            ActionType = actionType,
            Date = DateTime.UtcNow
        });
    }
}