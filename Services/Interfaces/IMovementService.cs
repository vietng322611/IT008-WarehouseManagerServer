using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IMovementService
{
    Task<List<Movement>> GetByWarehouseAsync(int warehouseId);

    Task<Movement?> GetByKeyAsync(int movementId);

    // Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters);
    Task<Movement> AddAsync(Movement movement);
    Task<Movement?> UpdateAsync(Movement movement);
    Task UpsertAsync(List<Movement> movements);
    Task<bool> DeleteAsync(int movementId);
}