using System.Linq.Expressions;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class MovementService(IMovementRepository movementRepository): IMovementService
{
    public Task<IEnumerable<Movement>> GetAllAsync() 
        => movementRepository.GetAllAsync();
    public Task<Movement?> GetByKeyAsync(int movementId) 
        => movementRepository.GetByKeyAsync(movementId);
    public Task<IEnumerable<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters)
        => movementRepository.FilterAsync(filters);
    public Task<Movement> AddAsync(Movement movement) 
        => movementRepository.AddAsync(movement);
    public Task<Movement?> UpdateAsync(Movement movement) 
        => movementRepository.UpdateAsync(movement);
    public Task<bool> DeleteAsync(int movementId)
        => movementRepository.DeleteAsync(movementId);
}