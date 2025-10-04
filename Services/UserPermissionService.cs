using System.Linq.Expressions;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class UserPermissionService(IUserPermissionRepository userPermissionRepository): IUserPermissionService
{
    public Task<IEnumerable<UserPermission>> GetAllAsync()
        => userPermissionRepository.GetAllAsync();
    public Task<UserPermission?> GetByKeyAsync(int userId, int warehouseId)
        => userPermissionRepository.GetByKeyAsync(userId, warehouseId);
    public Task<IEnumerable<UserPermission>> FilterAsync(params Expression<Func<UserPermission, bool>>[] filters)
        => userPermissionRepository.FilterAsync(filters);
    public Task<UserPermission> AddAsync(UserPermission userPermission)
        => userPermissionRepository.AddAsync(userPermission);
    public Task<UserPermission?> UpdateAsync(UserPermission userPermission)
        => userPermissionRepository.UpdateAsync(userPermission);
    public Task<bool> DeleteAsync(int userId, int warehouseId)
        => userPermissionRepository.DeleteAsync(userId, warehouseId);
}