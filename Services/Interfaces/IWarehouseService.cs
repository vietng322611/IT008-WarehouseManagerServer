using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IWarehouseService
{
    Task<IEnumerable<Warehouse>> GetAllAsync();
    Task<Warehouse?> GetByKeyAsync(int warehouseId);
    Task<IEnumerable<User>> GetWarehouseUsersAsync(int warehouseId);
    Task<Warehouse> AddAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(Warehouse warehouse);
    Task<bool> DeleteAsync(int warehouseId);
}