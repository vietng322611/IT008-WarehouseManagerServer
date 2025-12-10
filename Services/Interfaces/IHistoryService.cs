using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IHistoryService
{
    Task<List<History>> GetByWarehouseAsync(int warehouseId);

    Task<History?> GetByKeyAsync(int movementId);

    // Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters);
    Task<History> AddAsync(HistoryDto history);
    Task<History?> UpdateAsync(HistoryDto history);
    Task UpsertAsync(List<HistoryDto> movements);
    Task<bool> DeleteAsync(int movementId);
}