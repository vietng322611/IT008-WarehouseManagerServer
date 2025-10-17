using WarehouseManagerServer.Repositories;
using WarehouseManagerServer.Repositories.Interfaces;

namespace WarehouseManagerServer.Extensions;

public static class RepositoryExtension
{
    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMovementRepository, MovementRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
    }
}