using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class UserRepository(WarehouseContext context): IUserRepository
{
    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }
    
    public async Task<User?> GetByKeyAsync(int userId) 
    {
        return await context.Users.FindAsync(userId);
    }
    
    public async Task<IEnumerable<Warehouse>> GetUserWarehousesAsync(int userId)
    {
        return await context.Users
            .Where(e => e.UserId == userId)
            .SelectMany(u => u.Warehouses)
            .ToListAsync();
    }
    
    public async Task<User> AddAsync(User user) 
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User?> UpdateAsync(User user) 
    {
        var oldUser = await context.Users.FindAsync(user.UserId);
        if (oldUser == null) return null;
        
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }
    
    public async Task<bool> DeleteAsync(int userId) 
    {
        var oldUser = await context.Users.FindAsync(userId);
        if (oldUser == null) return false;

        context.Users.Remove(oldUser);
        return true;
    }
}