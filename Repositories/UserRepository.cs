using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class UserRepository(WarehouseContext context) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User?> GetByKeyAsync(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        return user;
    }

    public async Task<User?> GetByUniqueAsync(Expression<Func<User, bool>> condition)
    {
        var user = await context.Users.Where(condition).FirstOrDefaultAsync();
        if (user == null) return null;

        return new User
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            JoinDate = user.JoinDate
        };
    }

    public async Task<List<Warehouse>> GetUserWarehousesAsync(int userId)
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