using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class SupplierRepository(WarehouseContext context) : ISupplierRepository
{
    public async Task<List<Supplier>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Suppliers.Where(p => p.WarehouseId == warehouseId).ToListAsync();
    }

    public async Task<Supplier?> GetByKeyAsync(int supplierId)
    {
        return await context.Suppliers.FindAsync(supplierId);
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