using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class PermissionRepository(WarehouseContext context) : IPermissionRepository
{
    public async Task<bool> HasPermissionAsync(int userId, int warehouseId, PermissionEnum requiredPermission)
    {
        var permission = await context.Permissions
            .FirstOrDefaultAsync(p => p.UserId == userId && p.WarehouseId == warehouseId);
        
        return permission != null && (
            permission.UserPermissions.Contains(requiredPermission) || 
            permission.UserPermissions.Contains(PermissionEnum.Owner)
            );
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