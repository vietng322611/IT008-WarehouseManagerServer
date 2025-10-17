using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class UserService(IUserRepository userRepository): IUserService
{
    public Task<List<UserDto>> GetAllAsync()
        => userRepository.GetAllAsync();
    public Task<UserDto?> GetByKeyAsync(int userId)
        => userRepository.GetByKeyAsync(userId);
    public Task<List<Warehouse>> GetUserWarehousesAsync(int userId)
        => userRepository.GetUserWarehousesAsync(userId);
    public Task<UserDto> AddAsync(User user)
        => userRepository.AddAsync(user);
    public Task<UserDto?> UpdateAsync(User user)
        => userRepository.UpdateAsync(user);
    public Task<bool> DeleteAsync(int userId)
        => userRepository.DeleteAsync(userId);
}