using System.Linq.Expressions;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class PermissionService(IPermissionRepository permissionRepository): IPermissionService
{
    public Task<List<Permission>> GetAllAsync()
        => permissionRepository.GetAllAsync();
    public Task<Permission?> GetByKeyAsync(int userId, int warehouseId)
        => permissionRepository.GetByKeyAsync(userId, warehouseId);
    public Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters)
        => permissionRepository.FilterAsync(filters);
    public Task<Permission> AddAsync(Permission permission)
        => permissionRepository.AddAsync(permission);
    public Task<Permission?> UpdateAsync(Permission permission)
        => permissionRepository.UpdateAsync(permission);
    public Task<bool> DeleteAsync(int userId, int warehouseId)
        => permissionRepository.DeleteAsync(userId, warehouseId);
}