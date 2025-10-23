using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Services.Interfaces;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Controllers;

/* Route: api/Permission
 * Endpoints:
 *      - api/Permission: POST
 *      - api/Permission/json: GET
 *      - api/Permission/[userId]-[WarehouseId]: GET, PUT, DELETE
 *      - api/Permission/user/[UserId]: GET
 *      - api/Permission/warehouse/[WarehouseId]: GET
 */

[ApiController]
[Route("api/[controller]")]
public class PermissionController(IPermissionService service) : ControllerBase
{
    // [HttpGet]
    // public async Task<IActionResult> GetAll()
    // {
    //     var content = await service.GetAllAsync();
    //     return Ok(content);
    // }

    [HttpGet("json")]
    public IActionResult GetSampleJson()
    {
        var model = new Permission
        {
            UserId = 0,
            WarehouseId = 0,
            PermissionEnum = [PermissionEnum.Read, PermissionEnum.Write, PermissionEnum.Delete, PermissionEnum.Owner]
        };
        return Ok(model);
    }

    [HttpGet("{userId:int:min(1)}-{warehouseId:int:min(1)}")]
    public async Task<IActionResult> GetByIds([FromRoute] int userId, [FromRoute] int warehouseId)
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
    
    [HttpGet("user/{userId:int:min(1)}")]
    public async Task<IActionResult> GetByUserId([FromRoute] int userId)
    {
        try
        {
            var contents = await service.FilterAsync(e => e.UserId == userId);
            return Ok(contents);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet("warehouse/{warehouseId:int:min(1)}")]
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
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Permission content)
    {
        try
        {
            content.UserId = 0; // Ignore ids in input
            content.WarehouseId = 0;

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetByIds), new
            {
                userId = newContent.UserId,
                warehouseId = newContent.WarehouseId
            }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPut("{userId:int:min(1)}-{warehouseId:int:min(1)}")]
    public async Task<IActionResult> Put(
        [FromRoute] int userId,
        [FromRoute] int warehouseId,
        [FromBody] Permission updatedContent)
    {
        try
        {
            if (userId != updatedContent.UserId ||
                warehouseId != updatedContent.WarehouseId)
                return BadRequest();

            var existingContent = await service.GetByKeyAsync(userId, warehouseId);
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
    
    [HttpDelete("{userId:int:min(1)}-{warehouseId:int:min(1)}")]
    public async Task<IActionResult> Delete([FromRoute] int userId, [FromRoute] int warehouseId)
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