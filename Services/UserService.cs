using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Repositories.Interfaces;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public Task<List<User>> GetAllAsync()
        => userRepository.GetAllAsync();

    public Task<User?> GetByKeyAsync(int userId)
        => userRepository.GetByKeyAsync(userId);

    public Task<List<Warehouse>> GetUserWarehousesAsync(int userId)
        => userRepository.GetUserWarehousesAsync(userId);

    public Task<User> AddAsync(User user)
        => userRepository.AddAsync(user);

    public Task<User?> UpdateAsync(User user)
        => userRepository.UpdateAsync(user);

    public Task<bool> DeleteAsync(int userId)
        => userRepository.DeleteAsync(userId);
}