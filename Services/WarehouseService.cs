using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class WarehouseService(IWarehouseRepository warehouseRepository) : IWarehouseService
{
    public Task<Warehouse?> GetByKeyAsync(int warehouseId)
        => warehouseRepository.GetByKeyAsync(warehouseId);

    public Task<List<UserDto>> GetWarehouseUsersAsync(int warehouseId)
        => warehouseRepository.GetWarehouseUsersAsync(warehouseId);

    public Task<Warehouse> AddAsync(Warehouse warehouse)
        => warehouseRepository.AddAsync(warehouse);

    public Task<Warehouse?> UpdateAsync(Warehouse warehouse)
        => warehouseRepository.UpdateAsync(warehouse);

    public Task<bool> DeleteAsync(int warehouseId)
        => warehouseRepository.DeleteAsync(warehouseId);

    public Task<(bool, string)> Sync(int warehouseId, WarehouseSyncDto syncDto)
        => warehouseRepository.Sync(warehouseId, syncDto);
}