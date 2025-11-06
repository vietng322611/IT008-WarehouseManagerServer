using System.Linq.Expressions;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IPermissionRepository
{
    Task<bool> HasPermissionAsync(int userId, int warehouseId, PermissionEnum requiredPermissions);
    Task<Permission?> GetByKeyAsync(int userId, int warehouseId);
    Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters);
    Task<Permission> AddAsync(Permission permission);
    Task<Permission?> UpdateAsync(Permission permission);
    Task<bool> DeleteAsync(int userId, int warehouseId);
}