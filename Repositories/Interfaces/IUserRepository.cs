using System.Linq.Expressions;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Repositories.Interfaces;

public interface IUserRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByKeyAsync(int userId);
    Task<UserDto?> GetByUniqueAsync(Expression<Func<User, bool>> condition);
    Task<List<Warehouse>> GetUserWarehousesAsync(int userId);
    Task<UserDto> AddAsync(User user);
    Task<UserDto?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
}