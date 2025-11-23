using System.Linq.Expressions;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public Task<List<User>> GetAllAsync()
    {
        return userRepository.GetAllAsync();
    }

    public Task<User?> GetByKeyAsync(int userId)
    {
        return userRepository.GetByKeyAsync(userId);
    }

    public Task<User?> GetByUniqueAsync(Expression<Func<User, bool>> condition)
    {
        return userRepository.GetByUniqueAsync(condition);
    }

    public Task<List<Warehouse>> GetUserWarehousesAsync(int userId)
    {
        return userRepository.GetUserWarehousesAsync(userId);
    }

    public Task<User> AddAsync(User user)
    {
        return userRepository.AddAsync(user);
    }

    public Task<User?> UpdateAsync(User user)
    {
        return userRepository.UpdateAsync(user);
    }

    public Task<bool> DeleteAsync(int userId)
    {
        return userRepository.DeleteAsync(userId);
    }
}