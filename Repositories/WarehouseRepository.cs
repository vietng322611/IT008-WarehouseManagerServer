using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
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

    public async Task<List<UserDto>> GetWarehouseUsersAsync(int warehouseId)
    {
        return await context.Warehouses
            .Where(e => e.WarehouseId == warehouseId)
            .SelectMany(p => p.Users)
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
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
}