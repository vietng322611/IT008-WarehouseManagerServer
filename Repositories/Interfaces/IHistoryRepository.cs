using System.Linq.Expressions;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IHistoryRepository
{
    Task<List<History>> GetByWarehouseAsync(int warehouseId);
    Task<History?> GetByKeyAsync(int historyId);
    Task<List<History>> FilterAsync(params Expression<Func<History, bool>>[] filters);
}