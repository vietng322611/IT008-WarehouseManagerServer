using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehouseController(
    IWarehouseService service,
    IPermissionService permissionService
) : Controller
{
    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet("{warehouseId:int:min(1)}")]
    public async Task<IActionResult> GetById([FromRoute] int warehouseId)
    {
        try
        {
            var content = await service.GetByKeyAsync(warehouseId);
            if (content == null) return NotFound();
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet("{warehouseId:int:min(1)}/users")]
    public async Task<IActionResult> GetWarehouseUsers([FromRoute] int warehouseId)
    {
        try
        {
            var content = await service.GetWarehouseUsersAsync(warehouseId);
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.Authenticated)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Warehouse content)
    {
        try
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var userId = int.Parse(userIdClaim.Value);
            
            content.WarehouseId = 0; // Ignore id in input
            var newContent = await service.AddAsync(content);

            await permissionService.AddAsync(new Permission
            {
                UserId = userId,
                WarehouseId = newContent.WarehouseId,
                UserPermissions = [PermissionEnum.Owner]
            });
            return CreatedAtAction(nameof(GetById), new { warehouseId = newContent.WarehouseId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("{warehouseId:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int warehouseId, [FromBody] Warehouse updatedContent)
    {
        try
        {
            if (warehouseId != updatedContent.WarehouseId)
                return BadRequest();

            var existingContent = await service.GetByKeyAsync(warehouseId);
            if (existingContent == null)
                return NotFound();

            await service.UpdateAsync(updatedContent);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Delete)]
    [HttpDelete("{warehouseId:int:min(1)}")]
    public async Task<IActionResult> Delete([FromRoute] int warehouseId)
    {
        try
        {
            var success = await service.DeleteAsync(warehouseId);
            if (success)
                return NoContent();
            return NotFound();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}