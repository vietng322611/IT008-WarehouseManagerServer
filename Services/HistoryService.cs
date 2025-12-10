using System.Linq.Expressions;
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

    public Task<List<History>> FilterAsync(params Expression<Func<History, bool>>[] filters)
        => historyRepository.FilterAsync(filters);
}