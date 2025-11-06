using WarehouseManagerServer.Models.Entities;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByKeyAsync(int userId);
    Task<List<Warehouse>> GetUserWarehousesAsync(int userId);
    Task<User> AddAsync(User user);
    Task<User?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
}