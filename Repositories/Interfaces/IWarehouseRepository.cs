using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByKeyAsync(int warehouseId);
    Task<List<WarehouseUsersDto>> GetWarehouseUsersAsync(int warehouseId);
    Task<Warehouse> AddAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(Warehouse warehouse);
    Task<bool> DeleteAsync(int warehouseId);
}