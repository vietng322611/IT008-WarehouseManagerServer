using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class WarehouseRepository(WarehouseContext context) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await context.Warehouses.ToListAsync();
    }
    
    public async Task<Warehouse?> GetByKeyAsync(int warehouseId)
    {
        return await context.Warehouses.FindAsync(warehouseId);
    }

    public async Task<List<WarehouseUsersDto>> GetWarehouseUsersAsync(int warehouseId)
    {
        return await context.Permissions
            .Where(e => e.WarehouseId == warehouseId)
            .Select(u => new WarehouseUsersDto
            {
                UserId = u.UserId,
                FullName = u.User.FullName,
                Permissions = u.UserPermissions
            })
            .ToListAsync();
    }

    public async Task<Warehouse> AddAsync(Warehouse warehouse)
    {
        context.Warehouses.Add(warehouse);
        await context.SaveChangesAsync();
        return warehouse;
    }

    public async Task<Warehouse?> UpdateAsync(Warehouse warehouse)
    {
        var oldWarehouse = await context.Warehouses.FindAsync(warehouse.WarehouseId);
        if (oldWarehouse == null) return null;

        context.Entry(oldWarehouse).CurrentValues.SetValues(warehouse);
        await context.SaveChangesAsync();
        return warehouse;
    }

    public async Task<bool> DeleteAsync(int warehouseId)
    {
        var oldWarehouse = await context.Warehouses.FindAsync(warehouseId);
        if (oldWarehouse == null) return false;

        context.Warehouses.Remove(oldWarehouse);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<StatisticDto> GetStatisticsAsync(int warehouseId, int day, int month, int year)
    {
        var statistic = new StatisticDto();

        var supplierStats = await context.Histories
            .Include(m => m.Product)
            .ThenInclude(p => p.Supplier)
            .Where(m =>
                m.Product.WarehouseId == warehouseId &&
                m.ActionType == ActionTypeEnum.In &&
                m.Date.Year == year
            )
            .GroupBy(m => m.Product.Supplier != null ? m.Product.Supplier.Name : "Other")
            .Select(g => new SupplierStat
            {
                Name = g.Key,
                Count = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();
        
        var import = await context.Histories
            .Where(m =>
                m.Product.WarehouseId == warehouseId &&
                m.ActionType == ActionTypeEnum.In &&
                m.Date.Year == year
            )
            .SumAsync(m => m.Quantity);
        
        var sale = await context.Histories
            .Include(m => m.Product)
            .Where(m =>
                m.Product.WarehouseId == warehouseId &&
                m.ActionType == ActionTypeEnum.Out &&
                m.Date.Year == year
            )
            .GroupBy(m => m.Date.Month)
            .Select(g => new
            {
                Month = g.Key,
                TotalValue = g.Sum(x => x.Quantity * x.Product.UnitPrice),
                ProductCount = g.Sum(x=> x.Quantity),
                Count = g.Count()
            })
            .OrderBy(x => x.Month)
            .ToListAsync();

        statistic.SupplierStats = supplierStats;
        statistic.MonthlySale = Enumerable.Range(1, 12)
            .Select(m => sale.FirstOrDefault(x => x.Month == m)?.TotalValue ?? 0)
            .ToList();
        statistic.MonthlySaleCount = Enumerable.Range(1, 12)
            .Select(m => sale.FirstOrDefault(x => x.Month == m)?.ProductCount ?? 0)
            .ToList();
        statistic.Import = import;
        statistic.Export = sale.Sum(x => x.Count);

        return statistic;
    } 
}