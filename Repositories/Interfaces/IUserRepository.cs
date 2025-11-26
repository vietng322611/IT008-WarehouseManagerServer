using System.Linq.Expressions;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByKeyAsync(int userId);
    Task<User?> GetByUniqueAsync(Expression<Func<User, bool>> condition);
    Task<List<UserWarehousesDto>> GetUserWarehousesAsync(int userId);
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
}