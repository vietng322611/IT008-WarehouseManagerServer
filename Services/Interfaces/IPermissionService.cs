using System.Linq.Expressions;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(int userId, int warehouseId, PermissionEnum requiredPermission);
    Task<Permission?> GetByKeyAsync(int userId, int warehouseId);
    Task<List<Permission>> FilterAsync(params Expression<Func<Permission, bool>>[] filters);
    Task<Permission> AddAsync(Permission permission);
    Task<Permission?> AddByEmailAsync(int warehouseId, string email, List<PermissionEnum> permissions);
    Task<List<Permission>> UpdateAsync(int warehouseId, List<PermissionUpdDto> permissions);
    Task<bool> DeleteAsync(int userId, int warehouseId);
}