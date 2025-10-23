using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class PermissionRepository(WarehouseContext context) : IPermissionRepository
{
    public async Task<List<Permission>> GetAllAsync()
    {
        return await context.Permissions.ToListAsync();
    }

    public async Task<Permission?> GetByKeyAsync(int userId, int warehouseId)
    {
        return await context.Permissions.FindAsync(userId, warehouseId);
    }

    public async Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters)
    {
        var query = context.Permissions.AsQueryable();
        query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.ToListAsync();
    }

    public async Task<Permission> AddAsync(Permission permission)
    {
        context.Permissions.Add(permission);
        await context.SaveChangesAsync();
        return permission;
    }

    public async Task<Permission?> UpdateAsync(Permission permission)
    {
        var oldUserPermission = await GetByKeyAsync(permission.UserId, permission.WarehouseId);
        if (oldUserPermission == null) return null;

        context.Entry(permission).CurrentValues.SetValues(permission);
        await context.SaveChangesAsync();
        return permission;
    }

    public async Task<bool> DeleteAsync(int userId, int warehouseId)
    {
        var oldUserPermission = await GetByKeyAsync(userId, warehouseId);
        if (oldUserPermission == null) return false;

        context.Permissions.Remove(oldUserPermission);
        await context.SaveChangesAsync();
        return true;
    }
}