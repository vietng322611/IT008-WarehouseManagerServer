using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class MovementRepository(WarehouseContext context) : IMovementRepository
{
    public async Task<List<Movement>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Movements
            .Include(m => m.Product)
            .Where(m => m.Product.WarehouseId == warehouseId)
            .OrderBy(m => m.Date)
            .ToListAsync();
    }

    public async Task<Movement?> GetByKeyAsync(int movementId)
    {
        return await context.Movements
            .Include(m => m.Product)
            .Where(m => m.MovementId == movementId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Movement>> FilterAsync(params Expression<Func<Movement, bool>>[] filters)
    {
        var query = context.Movements.AsQueryable();
        query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.ToListAsync();
    }

    public async Task<Movement> AddAsync(Movement movement)
    {
        context.Movements.Add(movement);
        await context.SaveChangesAsync();
        return movement;
    }

    public async Task<Movement?> UpdateAsync(Movement movement)
    {
        var oldMovement = await GetByKeyAsync(movement.MovementId);
        if (oldMovement == null) return null;

        context.Entry(oldMovement).CurrentValues.SetValues(movement);
        await context.SaveChangesAsync();
        return movement;
    }
    
    public async Task UpsertAsync(List<Movement> movements)
    {
        foreach (var movement in movements)
        {
            var existing = await context.Movements.FindAsync(movement.MovementId);

            if (existing is null)
            {
                var newMovement = new Movement
                {
                    ProductId = movement.ProductId,
                    Quantity = movement.Quantity,
                    MovementType = movement.MovementType,
                    Date = movement.Date
                };
                context.Movements.Add(newMovement);
            }
            else
            {
                existing.ProductId = movement.ProductId;
                existing.MovementType = movement.MovementType;
                existing.Date = movement.Date;
            }
        }
    }

    public async Task<bool> DeleteAsync(int movementId)
    {
        var oldMovement = await GetByKeyAsync(movementId);
        if (oldMovement == null) return false;

        context.Movements.Remove(oldMovement);
        await context.SaveChangesAsync();
        return true;
    }
}