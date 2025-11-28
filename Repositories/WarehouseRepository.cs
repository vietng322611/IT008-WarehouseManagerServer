using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
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

    public async Task<List<WarehouseUsersDto>> GetWarehouseUsersAsync(int warehouseId)
    {
        return await context.Permissions
            .Where(e => e.WarehouseId == warehouseId)
            .Select(u => new WarehouseUsersDto
            {
                UserId = u.UserId,
                FullName = u.User.FullName,
                Permissions = u.UserPermissions
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
}