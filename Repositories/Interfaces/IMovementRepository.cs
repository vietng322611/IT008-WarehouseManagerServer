using System.Linq.Expressions;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IMovementRepository
{
    Task<List<Movement>> GetByWarehouseAsync(int warehouseId);
    Task<Movement?> GetByKeyAsync(int movementId);
    Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters);
    Task<Movement> AddAsync(Movement movement);
    Task<Movement?> UpdateAsync(Movement movement);
    Task UpsertMovements(List<Movement> movements);
    Task<bool> DeleteAsync(int movementId);
}