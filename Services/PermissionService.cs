using System.Linq.Expressions;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class PermissionService(IUserPermissionRepository userPermissionRepository): IUserPermissionService
{
    public Task<List<Permission>> GetAllAsync()
        => userPermissionRepository.GetAllAsync();
    public Task<Permission?> GetByKeyAsync(int userId, int warehouseId)
        => userPermissionRepository.GetByKeyAsync(userId, warehouseId);
    public Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters)
        => userPermissionRepository.FilterAsync(filters);
    public Task<Permission> AddAsync(Permission permission)
        => userPermissionRepository.AddAsync(permission);
    public Task<Permission?> UpdateAsync(Permission permission)
        => userPermissionRepository.UpdateAsync(permission);
    public Task<bool> DeleteAsync(int userId, int warehouseId)
        => userPermissionRepository.DeleteAsync(userId, warehouseId);
}