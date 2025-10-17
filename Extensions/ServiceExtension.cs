using WarehouseManagerServer.Services;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Extensions;

public static class ServiceExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMovementService, MovementService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
    }
}