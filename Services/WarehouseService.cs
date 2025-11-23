using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class WarehouseService(IWarehouseRepository warehouseRepository) : IWarehouseService
{
    public Task<Warehouse?> GetByKeyAsync(int warehouseId)
    {
        return warehouseRepository.GetByKeyAsync(warehouseId);
    }

    public Task<List<User>> GetWarehouseUsersAsync(int warehouseId)
    {
        return warehouseRepository.GetWarehouseUsersAsync(warehouseId);
    }

    public Task<Warehouse> AddAsync(Warehouse warehouse)
    {
        return warehouseRepository.AddAsync(warehouse);
    }

    public Task<Warehouse?> UpdateAsync(Warehouse warehouse)
    {
        return warehouseRepository.UpdateAsync(warehouse);
    }

    public Task<bool> DeleteAsync(int warehouseId)
    {
        return warehouseRepository.DeleteAsync(warehouseId);
    }
}