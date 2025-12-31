using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class HistoryRepository(WarehouseContext context) : IHistoryRepository
{
    public async Task<List<History>> GetByWarehouseAsync(int warehouseId)
    {
        return await context.Histories.AsNoTracking()
            .Where(m => m.WarehouseId == warehouseId)
            .OrderByDescending(m => m.Date)
            .ToListAsync();
    }

    public async Task<History?> GetByKeyAsync(int historyId)
    {
        return await context.Histories.AsNoTracking()
            .Where(m => m.HistoryId == historyId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<History>> FilterAsync(params Expression<Func<History, bool>>[] filters)
    {
        var query = context.Histories.AsQueryable();
        query = filters.Aggregate(query, (current, filter) => current.Where(filter));
        return await query.ToListAsync();
    }
}