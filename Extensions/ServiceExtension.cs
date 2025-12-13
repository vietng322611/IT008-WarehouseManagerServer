using WarehouseManagerServer.Services;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Extensions;

public static class ServiceExtension
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
    }
}