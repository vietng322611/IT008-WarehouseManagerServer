using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int:min(1)}/movements")]
public class MovementController(IMovementService service) : ControllerBase
{
    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet]
    public async Task<IActionResult> GetWarehouseMovements([FromRoute] int id)
    {
        var result = await service.GetByWarehouseAsync(id);
        return Ok(result.Select(Serialize));
    }

    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var content = await service.GetByKeyAsync(id);
            if (content == null) return NotFound();
            return Ok(Serialize(content));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Movement content)
    {
        try
        {
            content.MovementId = 0; // Ignore id in input
            await service.AddAsync(content);

            var newContent = await service.GetByKeyAsync(content.MovementId);
            if (newContent == null) return StatusCode(500, "Error adding new content");

            return CreatedAtAction(
                nameof(GetById),
                new { id = newContent.MovementId },
                Serialize(newContent));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [WarehousePermission(PermissionEnum.Write)]
    [HttpPost("upsert")]
    public async Task<IActionResult> Upsert([FromBody] List<Movement> contents)
    {
        try
        {
            await service.UpsertAsync(contents);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Movement updatedContent)
    {
        try
        {
            if (id != updatedContent.MovementId)
                return BadRequest();

            var existingContent = await service.GetByKeyAsync(id);
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

    [WarehousePermission(PermissionEnum.Write)]
    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        try
        {
            var success = await service.DeleteAsync(id);
            if (success)
                return NoContent();
            return NotFound();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    private object Serialize(Movement content)
    {
        return new
        {
            movement_id = content.MovementId,
            product_id = content.ProductId,
            product_name = content.Product.Name,
            quantity = content.Quantity,
            movement_type = content.MovementType,
            date = content.Date
        };
    }
}