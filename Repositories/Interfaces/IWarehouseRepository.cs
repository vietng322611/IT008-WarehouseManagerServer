using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IWarehouseRepository
{
    Task<Warehouse?> GetByKeyAsync(int warehouseId);
    Task<List<WarehouseUsersDto>> GetWarehouseUsersAsync(int warehouseId);
    Task<Warehouse> AddAsync(Warehouse warehouse);
    Task<Warehouse?> UpdateAsync(Warehouse warehouse);
    Task<bool> DeleteAsync(int warehouseId);
    Task<StatisticDto> GetMonthlyStatAsync(int warehouseId, int month, int year);
    Task<StatisticDto> GetYearlyStatAsync(int warehouseId, int year);
}