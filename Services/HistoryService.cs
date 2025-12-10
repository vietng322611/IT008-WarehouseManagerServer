using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class HistoryService(IHistoryRepository historyRepository) : IHistoryService
{
    public Task<List<History>> GetByWarehouseAsync(int warehouseId)
        => historyRepository.GetByWarehouseAsync(warehouseId);

    public Task<History?> GetByKeyAsync(int movementId)
        => historyRepository.GetByKeyAsync(movementId);

    // public Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters)
    //     => movementRepository.FilterAsync(filters);

    public Task<History> AddAsync(HistoryDto history)
        => historyRepository.AddAsync(history);

    public Task<History?> UpdateAsync(HistoryDto history)
        => historyRepository.UpdateAsync(history);

    public Task UpsertAsync(List<HistoryDto> movements)
        => historyRepository.UpsertAsync(movements);

    public Task<bool> DeleteAsync(int movementId)
        => historyRepository.DeleteAsync(movementId);
}