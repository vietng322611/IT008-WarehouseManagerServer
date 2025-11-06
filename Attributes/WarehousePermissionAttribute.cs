using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Attributes;

public class WarehousePermissionAttribute(
    PermissionEnum requiredPermission
) : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();
        var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        // Extract warehouseId from route values (must exist in route template)
        if (!context.RouteData.Values.TryGetValue("warehouseId", out var idObj) ||
            !int.TryParse(idObj?.ToString(), out var warehouseId))
        {
            context.Result = new BadRequestObjectResult("Missing or invalid warehouseId in route.");
            return;
        }

        var hasPermission = await permissionService.HasPermissionAsync(userId, warehouseId, requiredPermission);
        if (!hasPermission)
            context.Result = new ForbidResult();
    }
}