using System.Linq.Expressions;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class PermissionService(IPermissionRepository permissionRepository) : IPermissionService
{
    public Task<bool> HasPermissionAsync(int userId, int warehouseId, PermissionEnum requiredPermission)
    {
        return permissionRepository.HasPermissionAsync(userId, warehouseId, requiredPermission);
    }

    public Task<Permission?> GetByKeyAsync(int userId, int warehouseId)
    {
        return permissionRepository.GetByKeyAsync(userId, warehouseId);
    }

    public Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters)
    {
        return permissionRepository.FilterAsync(filters);
    }

    public Task<Permission> AddAsync(Permission permission)
    {
        return permissionRepository.AddAsync(permission);
    }

    public Task<Permission?> AddByEmailAsync(int warehouseId, string email, List<PermissionEnum> permissions)
    {
        return permissionRepository.AddByEmailAsync(warehouseId, email, permissions);
    }

    public Task<List<Permission>> UpdateAsync(int warehouseId, List<Permission> permissions)
    {
        return permissionRepository.UpdateAsync(warehouseId, permissions);
    }

    public Task<bool> DeleteAsync(int userId, int warehouseId)
    {
        return permissionRepository.DeleteAsync(userId, warehouseId);
    }
}