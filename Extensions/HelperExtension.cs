using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Extensions;

public static class HelperExtension
{
    public static async Task UpsertProducts(WarehouseContext context, List<Product> products, int warehouseId)
    {
        foreach (var dto in products)
        {
            var existing = await context.Products.FindAsync(dto.ProductId);

            if (existing is null)
            {
                var newProduct = new Product
                {
                    Name = dto.Name,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    WarehouseId = warehouseId,
                    SupplierId = dto.SupplierId,
                    CategoryId = dto.CategoryId
                };
                context.Products.Add(newProduct);
            }
            else
            {
                existing.Name = dto.Name;
                existing.Quantity = dto.Quantity;
                existing.UnitPrice = dto.UnitPrice;
                existing.SupplierId = dto.SupplierId;
                existing.CategoryId = dto.CategoryId;
            }
        }
    }
    
    public static async Task UpsertCategories(WarehouseContext context, List<Category> categories, int warehouseId)
    {
        foreach (var dto in categories)
        {
            var existing = await context.Categories.FindAsync(dto.CategoryId);

            if (existing is null)
            {
                var newCategory = new Category
                {
                    WarehouseId = warehouseId,
                    Name = dto.Name
                };
                context.Categories.Add(newCategory);
            }
            else
            {
                existing.CategoryId = dto.CategoryId;
                existing.Name = dto.Name;
            }
        }
    }
    
    public static async Task UpsertMovements(WarehouseContext context, List<Movement> movements)
    {
        foreach (var dto in movements)
        {
            var existing = await context.Movements.FindAsync(dto.MovementId);

            if (existing is null)
            {
                var newMovement = new Movement
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    MovementType = dto.MovementType,
                    Date = dto.Date
                };
                context.Movements.Add(newMovement);
            }
            else
            {
                existing.ProductId = dto.ProductId;
                existing.MovementType = dto.MovementType;
                existing.Date = dto.Date;
            }
        }
    }
    
    public static async Task UpsertSuppliers(WarehouseContext context, List<Supplier> suppliers, int warehouseId)
    {
        foreach (var dto in suppliers)
        {
            var existing = await context.Suppliers.FindAsync(dto.SupplierId);

            if (existing is null)
            {
                var newSupplier = new Supplier
                {
                    WarehouseId = warehouseId,
                    Name = dto.Name,
                    ContactInfo = dto.ContactInfo
                };
                context.Suppliers.Add(newSupplier);
            }
            else
            {
                existing.Name = dto.Name;
                existing.WarehouseId = dto.WarehouseId;
                existing.ContactInfo = dto.ContactInfo;
            }
        }
    }
}