using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class UserPermissionRepository(WarehouseContext context): IUserPermissionRepository
{
    public async Task<IEnumerable<UserPermission>> GetAllAsync()
    {
        return await context.UserPermissions.ToListAsync();
    }
    
    public async Task<UserPermission?> GetByKeyAsync(int userId, int warehouseId) 
    {
        return await context.UserPermissions.FindAsync(userId, warehouseId);
    }
    
    public async Task<IEnumerable<UserPermission>> FilterAsync(params Expression<Func<UserPermission, bool>>[] filters) 
    {
        var query = context.UserPermissions.AsQueryable();
        query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.ToListAsync();
    }
    
    public async Task<UserPermission> AddAsync(UserPermission userPermission) 
    {
        context.UserPermissions.Add(userPermission);
        await context.SaveChangesAsync();
        return userPermission;
    }
    
    public async Task<UserPermission?> UpdateAsync(UserPermission userPermission) 
    {
        var oldUserPermission = await GetByKeyAsync(userPermission.UserId, userPermission.WarehouseId);
        if (oldUserPermission == null) return null;
        
        context.Entry(userPermission).CurrentValues.SetValues(userPermission);
        await context.SaveChangesAsync();
        return userPermission;
    }
    
    public async Task<bool> DeleteAsync(int userId, int warehouseId)
    {
        var oldUserPermission = await GetByKeyAsync(userId, warehouseId);
        if (oldUserPermission == null) return false;

        context.UserPermissions.Remove(oldUserPermission);
        await context.SaveChangesAsync();
        return true;
    }
    
}