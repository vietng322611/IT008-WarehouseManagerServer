using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;

namespace WarehouseManagerServer.Services.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByKeyAsync(int userId);
    Task<List<Warehouse>> GetUserWarehousesAsync(int userId);
    Task<UserDto> AddAsync(User user);
    Task<UserDto?> UpdateAsync(User user);
    Task<bool> DeleteAsync(int userId);
}