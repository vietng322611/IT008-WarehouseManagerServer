using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IMovementService
{
    Task<List<Movement>> GetByWarehouseAsync(int warehouseId);

    Task<Movement?> GetByKeyAsync(int movementId);

    // Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters);
    Task<Movement> AddAsync(MovementDto movement);
    Task<Movement?> UpdateAsync(MovementDto movement);
    Task UpsertAsync(List<MovementDto> movements);
    Task<bool> DeleteAsync(int movementId);
}