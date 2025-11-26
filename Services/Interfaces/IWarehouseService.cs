using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IWarehouseService
{
    Task<Warehouse?> GetByKeyAsync(int warehouseId);
    Task<List<WarehouseUsersDto>> GetWarehouseUsersAsync(int warehouseId);
    Task<Warehouse> AddAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(Warehouse warehouse);
    Task<bool> DeleteAsync(int warehouseId);
}