using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class SupplierRepository(WarehouseContext context) : ISupplierRepository
{
    public async Task<List<Supplier>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Suppliers
            .Where(p => p.WarehouseId == warehouseId)
            .Select(p => new Supplier
            {
                SupplierId = p.SupplierId,
                WarehouseId = p.WarehouseId,
                Name = p.Name,
                ContactInfo = p.ContactInfo,
                ProductCount = p.Products.Count
            })
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByKeyAsync(int supplierId)
    {
        return await context.Suppliers
            .Where(s => s.SupplierId == supplierId)
            .Select(p => new Supplier
            {
                SupplierId = p.SupplierId,
                WarehouseId = p.WarehouseId,
                Name = p.Name,
                ContactInfo = p.ContactInfo,
                ProductCount = p.Products.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<Supplier> AddAsync(Supplier supplier)
    {
        context.Suppliers.Add(supplier);
        await context.SaveChangesAsync();
        return supplier;
    }

    public async Task<Supplier?> UpdateAsync(Supplier supplier)
    {
        var oldSupplier = await GetByKeyAsync(supplier.SupplierId);
        if (oldSupplier == null) return null;

        context.Entry(oldSupplier).CurrentValues.SetValues(supplier);
        await context.SaveChangesAsync();
        return supplier;
    }

    public async Task<bool> DeleteAsync(int supplierId)
    {
        var oldSupplier = await GetByKeyAsync(supplierId);
        if (oldSupplier == null) return false;

        context.Suppliers.Remove(oldSupplier);
        await context.SaveChangesAsync();
        return true;
    }
}