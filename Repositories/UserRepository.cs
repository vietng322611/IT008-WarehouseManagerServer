using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Repositories;

public class UserRepository(WarehouseContext context) : IUserRepository
{
    public async Task<List<User>> GetAllAsync()
    {
        return await context.Users.Select(user => new User
        {
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            JoinDate = user.JoinDate
        }).ToListAsync();
    }

    public async Task<User?> GetByKeyAsync(int userId)
        => await context.Users.FindAsync(userId);

    public async Task<User?> GetByUniqueAsync(Expression<Func<User, bool>> condition)
        => await context.Users.Where(condition).FirstOrDefaultAsync();

    public async Task<List<UserWarehousesDto>> GetUserWarehousesAsync(int userId)
    {
        return await context.Permissions
            .Where(e => e.UserId == userId)
            .Select(e => new UserWarehousesDto
            {
                WarehouseId = e.WarehouseId,
                Name = e.Warehouse.Name,
                CreateDate = e.Warehouse.CreateDate,
                Permissions = e.UserPermissions
            })
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

    public async Task ChangePassword(User user, string newPasswordHash)
    {
        user.PasswordHash = newPasswordHash;
        await context.SaveChangesAsync();
    }

    public async Task<User?> GetUserFromToken(string refreshToken)
    {
        return await context.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
    }

    public async Task AddTokenAsync(User user, RefreshToken refreshToken)
    {
        user.RefreshTokens.Add(refreshToken);
        await context.SaveChangesAsync();
    }
    
    public async Task InvalidateRefreshToken(RefreshToken refreshToken)
    {
        context.RefreshTokens.Remove(refreshToken);
        await context.SaveChangesAsync();
    }
    
    public async Task ClearOutdatedAsync()
    {
        context.RefreshTokens.RemoveRange(
            context.RefreshTokens.Where(t => t.ExpiresAt < DateTime.UtcNow)
        );
        await context.SaveChangesAsync();
    }
}