using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class HistoryRepository(WarehouseContext context) : IHistoryRepository
{
    public async Task<List<History>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Histories
            .Include(m => m.Product)
            .Where(m => m.Product.WarehouseId == warehouseId)
            .OrderBy(m => m.Date)
            .ToListAsync();
    }

    public async Task<History?> GetByKeyAsync(int historyId)
    {
        return await context.Histories
            .Include(m => m.Product)
            .Where(m => m.HistoryId == historyId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<History>> FilterAsync(params Expression<Func<History, bool>>[] filters)
    {
        var query = context.Histories.AsQueryable();
        query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.ToListAsync();
    }

    public async Task<History> AddAsync(HistoryDto history)
    {
        var newMovement = context.Histories.Add(new History
        {
            ProductId = history.ProductId,
            Quantity = history.Quantity,
            ActionType = history.ActionType,
            Date = history.Date
        });
        await context.SaveChangesAsync();
        return newMovement.Entity;
    }

    public async Task<History?> UpdateAsync(HistoryDto history)
    {
        var oldHistory = await GetByKeyAsync(history.HistoryId);
        if (oldHistory == null) return null;

        oldHistory.ProductId = history.ProductId;
        oldHistory.Quantity = history.Quantity;
        oldHistory.ActionType = history.ActionType;
        oldHistory.Date = history.Date;
        
        await context.SaveChangesAsync();
        return oldHistory;
    }
    
    public async Task UpsertAsync(List<HistoryDto> histories)
    {
        foreach (var history in histories)
            await UpdateAsync(history);
    }

    public async Task<bool> DeleteAsync(int historyId)
    {
        var oldHistory = await GetByKeyAsync(historyId);
        if (oldHistory == null) return false;

        context.Histories.Remove(oldHistory);
        await context.SaveChangesAsync();
        return true;
    }
}