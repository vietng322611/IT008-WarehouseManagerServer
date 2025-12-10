using System.Linq.Expressions;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IHistoryService
{
    Task<List<History>> GetByWarehouseAsync(int warehouseId);
    Task<History?> GetByKeyAsync(int movementId);
    // Task<List<History>> FilterAsync(params Expression<Func<History, bool>>[] filters);
}