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

    public async Task<StatisticDto> GetMonthlyStatAsync(int warehouseId, int month, int year)
    {
        var statistic = new StatisticDto();

        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);
        var lastDay = DateTime.DaysInMonth(year, month);

        var raw = await context.Histories.AsNoTracking()
            .Where(m =>
                m.Product.WarehouseId == warehouseId &&
                m.Date >= start &&
                m.Date < end
            )
            .Select(m => new
            {
                m.ActionType,
                m.Date.Day,
                m.Quantity,
                SupplierName = m.Product.Supplier.Name
            })
            .ToListAsync();
        
        statistic.SupplierStats = raw
            .Where(x => x.ActionType == ActionTypeEnum.In)
            .GroupBy(x => x.SupplierName)
            .Select(g => new SupplierStat
            {
                Name = g.Key,
                Count = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.Count)
            .ToList();
        
        statistic.Sale = Enumerable.Range(1, lastDay)
            .Select(day =>
                raw.Where(x =>
                        x.ActionType is ActionTypeEnum.Out or ActionTypeEnum.Transfer &&
                        x.Day == day)
                    .Sum(x => x.Quantity))
            .ToList();

        statistic.Expired = Enumerable.Range(1, lastDay)
            .Select(day =>
                raw.Where(x =>
                        x.ActionType == ActionTypeEnum.Remove &&
                        x.Day == day)
                    .Sum(x => x.Quantity))
            .ToList();
        
        statistic.Import = raw
            .Where(x => x.ActionType == ActionTypeEnum.In)
            .Sum(x => x.Quantity);
        
        statistic.Export = raw.Count(x =>
            x.ActionType is ActionTypeEnum.Out or ActionTypeEnum.Transfer
        );

        return statistic;
    }
    
    public async Task<StatisticDto> GetYearlyStatAsync(int warehouseId, int year)
    {
        var statistic = new StatisticDto();

        var start = new DateTime(year, 1, 1);
        var end = start.AddYears(1);

        var raw = await context.Histories.AsNoTracking()
            .Where(m =>
                m.Product.WarehouseId == warehouseId &&
                m.Date >= start &&
                m.Date < end
            )
            .Select(m => new
            {
                m.ActionType,
                m.Date.Month,
                m.Quantity,
                SupplierName = m.Product.Supplier.Name
            })
            .ToListAsync();
        
        statistic.SupplierStats = raw
            .Where(x => x.ActionType == ActionTypeEnum.In)
            .GroupBy(x => x.SupplierName)
            .Select(g => new SupplierStat
            {
                Name = g.Key,
                Count = g.Sum(x => x.Quantity)
            })
            .OrderByDescending(x => x.Count)
            .ToList();
        
        statistic.Sale = Enumerable.Range(1, 12)
            .Select(month =>
                raw.Where(x =>
                        x.ActionType is ActionTypeEnum.Out or ActionTypeEnum.Transfer &&
                        x.Month == month)
                    .Sum(x => x.Quantity))
            .ToList();

        statistic.Expired = Enumerable.Range(1, 12)
            .Select(month =>
                raw.Where(x =>
                        x.ActionType == ActionTypeEnum.Remove &&
                        x.Month == month)
                    .Sum(x => x.Quantity))
            .ToList();
        
        statistic.Import = raw
            .Where(x => x.ActionType == ActionTypeEnum.In)
            .Sum(x => x.Quantity);
        
        statistic.Export = raw.Count(x =>
            x.ActionType is ActionTypeEnum.Out or ActionTypeEnum.Transfer
        );

        return statistic;
    }
}