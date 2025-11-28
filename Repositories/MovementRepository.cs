using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
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

    public async Task<Movement> AddAsync(MovementDto movement)
    {
        var newMovement = context.Movements.Add(new Movement
        {
            ProductId = movement.ProductId,
            Quantity = movement.Quantity,
            MovementType = movement.MovementType,
            Date = movement.Date
        });
        await context.SaveChangesAsync();
        return newMovement.Entity;
    }

    public async Task<Movement?> UpdateAsync(MovementDto movement)
    {
        var oldMovement = await GetByKeyAsync(movement.MovementId);
        if (oldMovement == null) return null;

        oldMovement.ProductId = movement.ProductId;
        oldMovement.Quantity = movement.Quantity;
        oldMovement.MovementType = movement.MovementType;
        oldMovement.Date = movement.Date;
        
        await context.SaveChangesAsync();
        return oldMovement;
    }
    
    public async Task UpsertAsync(List<MovementDto> movements)
    {
        foreach (var movement in movements)
            await UpdateAsync(movement);
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