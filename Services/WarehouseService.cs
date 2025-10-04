using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class WarehouseService(IWarehouseRepository warehouseRepository): IWarehouseService
{
    public Task<IEnumerable<Warehouse>> GetAllAsync()
        => warehouseRepository.GetAllAsync();
    public Task<Warehouse?> GetByKeyAsync(int warehouseId)
        => warehouseRepository.GetByKeyAsync(warehouseId);
    public Task<IEnumerable<User>> GetWarehouseUsersAsync(int warehouseId)
        => warehouseRepository.GetWarehouseUsersAsync(warehouseId);
    public Task<Warehouse> AddAsync(Warehouse warehouse)
        => warehouseRepository.AddAsync(warehouse);
    public Task<Warehouse?> UpdateAsync(Warehouse warehouse)
        => warehouseRepository.UpdateAsync(warehouse);
    public Task<bool> DeleteAsync(int warehouseId)
        => warehouseRepository.DeleteAsync(warehouseId);
}