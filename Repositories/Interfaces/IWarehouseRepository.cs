using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<List<Warehouse>> GetAllAsync();
    Task<Warehouse?> GetByKeyAsync(int warehouseId);
    Task<List<UserDto>> GetWarehouseUsersAsync(int warehouseId);
    Task<Warehouse> AddAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(Warehouse warehouse);
    Task<bool> DeleteAsync(int warehouseId);
}