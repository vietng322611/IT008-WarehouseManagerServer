using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByKeyAsync(int userId);
    Task<IEnumerable<Warehouse>> GetUserWarehousesAsync(int userId);
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
}