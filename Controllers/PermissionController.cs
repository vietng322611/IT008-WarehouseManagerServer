using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.DTOs.Requests;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int:min(1)}/permissions")]
public class PermissionController(IPermissionService service): ControllerBase
{
    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet]
    public async Task<IActionResult> GetByWarehouseId([FromRoute] int warehouseId)
    {
        try
        {
            var contents = await service.FilterAsync(e => e.WarehouseId == warehouseId);
            return Ok(contents);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet("{userId:int:min(1)}")]
    public async Task<IActionResult> GetByKey([FromRoute] int warehouseId, [FromRoute] int userId)
    {
        try
        {
            var content = await service.GetByKeyAsync(userId, warehouseId);
            if (content == null) return NotFound();
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Owner)]
    [HttpPost]
    public async Task<IActionResult> Post([FromRoute] int warehouseId, [FromBody] PermissionDto content)
    {
        try
        {
            var newContent = await service.AddByEmailAsync(warehouseId, content.Email, content.UserPermissions);
            if (newContent == null) return BadRequest();
            
            return CreatedAtAction(
                nameof(GetByKey), new {warehouseId, userId = newContent.UserId},  newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Owner)]
    [HttpPut("{userId:int:min(1)}")]
    public async Task<IActionResult> Put(
        [FromRoute] int warehouseId,
        [FromRoute] int userId,
        [FromBody] Permission updatedContent)
    {
        try
        {
            if (userId != updatedContent.UserId ||
                warehouseId != updatedContent.WarehouseId)
                return BadRequest();

            var newContent = await service.UpdateAsync(updatedContent);
            if (newContent == null)
                return NotFound();
            return Ok(newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Owner)]
    [HttpDelete("{userId:int:min(1)}")]
    public async Task<IActionResult> Delete([FromRoute] int warehouseId, [FromRoute] int userId)
    {
        try
        {
            var success = await service.DeleteAsync(userId, warehouseId);
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