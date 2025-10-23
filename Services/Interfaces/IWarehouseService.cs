using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IWarehouseService
{
    Task<List<Warehouse>> GetAllAsync();
    Task<Warehouse?> GetByKeyAsync(int warehouseId);
    Task<List<UserDto>> GetWarehouseUsersAsync(int warehouseId);
    Task<Warehouse> AddAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(Warehouse warehouse);
    Task<bool> DeleteAsync(int warehouseId);
}