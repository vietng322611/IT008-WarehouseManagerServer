using System.Linq.Expressions;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IUserPermissionRepository
{
    Task<IEnumerable<UserPermission>> GetAllAsync();
    Task<UserPermission?> GetByKeyAsync(int userId, int warehouseId);
    Task<IEnumerable<UserPermission>> FilterAsync(params Expression<Func<UserPermission, bool>>[] filters);
    Task<UserPermission> AddAsync(UserPermission userPermission);
    Task<UserPermission?> UpdateAsync(UserPermission userPermission);
    Task<bool> DeleteAsync(int userId, int warehouseId);
}