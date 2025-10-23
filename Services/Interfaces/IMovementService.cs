using System.Linq.Expressions;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IMovementService
{
    Task<List<Movement>> GetAllAsync();
    Task<Movement?> GetByKeyAsync(int movementId);
    Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters);
    Task<Movement> AddAsync(Movement movement);
    Task<Movement?> UpdateAsync(Movement movement);
    Task<bool> DeleteAsync(int movementId);
}