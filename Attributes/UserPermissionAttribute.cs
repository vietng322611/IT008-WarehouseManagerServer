using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Attributes;

public class UserPermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (!user.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        if (!context.RouteData.Values.TryGetValue("userId", out var idObj) ||
            !int.TryParse(idObj?.ToString(), out var requestUserId))
        {
            context.Result = new BadRequestObjectResult("Missing or invalid userId in route.");
            return;
        }

        if (userId != requestUserId)
            context.Result = new ForbidResult();
    }
}