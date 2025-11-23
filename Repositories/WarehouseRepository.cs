using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Extensions;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class WarehouseRepository(WarehouseContext context) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await context.Warehouses.ToListAsync();
    }

    public async Task<Warehouse?> GetByKeyAsync(int warehouseId)
    {
        return await context.Warehouses.FindAsync(warehouseId);
    }

    public async Task<List<User>> GetWarehouseUsersAsync(int warehouseId)
    {
        return await context.Warehouses
            .Where(e => e.WarehouseId == warehouseId)
            .SelectMany(p => p.Users)
            .Select(u => new User
            {
                UserId = u.UserId,
                FullName = u.FullName,
                Email = u.Email,
                JoinDate = u.JoinDate
            })
            .ToListAsync();
    }

    public async Task<Warehouse> AddAsync(Warehouse warehouse)
    {
        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();
        return warehouse;
    }

    public async Task<Warehouse?> UpdateAsync(Warehouse warehouse)
    {
        var oldWarehouse = await context.Warehouses.FindAsync(warehouse.WarehouseId);
        if (oldWarehouse == null) return null;

        context.Entry(oldWarehouse).CurrentValues.SetValues(warehouse);
        await context.SaveChangesAsync();
        return warehouse;
    }

    public async Task<bool> DeleteAsync(int warehouseId)
    {
        var oldWarehouse = await context.Warehouses.FindAsync(warehouseId);
        if (oldWarehouse == null) return false;

        context.Warehouses.Remove(oldWarehouse);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool, string)> Sync(int warehouseId, WarehouseSyncDto syncDto)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await HelperExtension.UpsertCategories(context, syncDto.Categories, warehouseId);
            await HelperExtension.UpsertSuppliers(context, syncDto.Suppliers, warehouseId);
            await HelperExtension.UpsertProducts(context, syncDto.Products, warehouseId);
            await HelperExtension.UpsertMovements(context, syncDto.Movements);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, ex.Message);
        }
    }
}