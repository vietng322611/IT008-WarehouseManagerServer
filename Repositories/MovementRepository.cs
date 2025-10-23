using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class MovementRepository(WarehouseContext context) : IMovementRepository
{
    public async Task<List<Movement>> GetAllAsync()
    {
        return await context.Movements.ToListAsync();
    }

    public async Task<Movement?> GetByKeyAsync(int movementId)
    {
        return await context.Movements.FindAsync(movementId);
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

    public async Task<bool> DeleteAsync(int movementId)
    {
        var oldMovement = await GetByKeyAsync(movementId);
        if (oldMovement == null) return false;

        context.Movements.Remove(oldMovement);
        await context.SaveChangesAsync();
        return true;
    }
}