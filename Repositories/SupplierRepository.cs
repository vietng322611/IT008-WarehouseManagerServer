using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
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

    public async Task<Supplier> AddAsync(SupplierDto supplier)
    {
        var newSupplier = context.Suppliers.Add(new Supplier
        {
            WarehouseId = supplier.WarehouseId,
            Name = supplier.Name,
            ContactInfo = supplier.ContactInfo,
        });
        await context.SaveChangesAsync();
        return newSupplier.Entity;
    }

    public async Task<Supplier?> UpdateAsync(SupplierDto supplier)
    {
        var oldSupplier = await GetByKeyAsync(supplier.SupplierId);
        if (oldSupplier == null) return null;

        oldSupplier.Name = supplier.Name;
        oldSupplier.ContactInfo = supplier.ContactInfo;
        
        await context.SaveChangesAsync();
        return oldSupplier;
    }

    public async Task UpsertAsync(List<SupplierDto> suppliers)
    {
        foreach (var supplier in suppliers)
        {
            var existing = await UpdateAsync(supplier);
            if (existing is null)
                await AddAsync(supplier);
        }
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