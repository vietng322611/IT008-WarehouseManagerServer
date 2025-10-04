using System.Linq.Expressions;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IMovementService
{
    Task<IEnumerable<Movement>> GetAllAsync();
    Task<Movement?> GetByKeyAsync(int movementId);
    Task<IEnumerable<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters);
    Task<Movement> AddAsync(Movement movement);
    Task<Movement?> UpdateAsync(Movement movement);
    Task<bool> DeleteAsync(int movementId);
}