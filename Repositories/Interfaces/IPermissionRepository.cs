using System.Linq.Expressions;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IPermissionRepository
{
    Task<List<Permission>> GetAllAsync();
    Task<Permission?> GetByKeyAsync(int userId, int warehouseId);
    Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters);
    Task<Permission> AddAsync(Permission permission);
    Task<Permission?> UpdateAsync(Permission permission);
    Task<bool> DeleteAsync(int userId, int warehouseId);
}