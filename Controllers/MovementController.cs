using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouse/{warehouseId:int:min(1)}/movements")]
public class MovementController(IMovementService service) : ControllerBase
{
    [HttpGet("json")]
    public IActionResult GetSampleJson()
    {
        var model = new
        {
            MovementId = 0,
            Product = "Product",
            Quantity = 1,
            MovementType = MovementTypeEnum.In,
            Date = DateTime.Now,
            ProductId = 0
        };
        return Ok(model);
    }

    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet]
    public async Task<IActionResult> GetWarehouseMovements([FromRoute] int id)
    {
        var result = await service.GetByWarehouseAsync(id);
        return Ok(result.Select(content => new
        {
            content.MovementId,
            Product = content.Product.Name,
            content.Quantity,
            content.MovementType,
            content.Date,
            content.ProductId
        }));
    }

    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var content = await service.GetByKeyAsync(id);
            if (content == null) return NotFound();
            return Ok(new
            {
                content.MovementId,
                Product = content.Product.Name,
                content.Quantity,
                content.MovementType,
                content.Date,
                content.ProductId
            });
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
                new
                {
                    newContent.MovementId,
                    Product = newContent.Product.Name,
                    newContent.Quantity,
                    newContent.MovementType,
                    newContent.Date,
                    newContent.ProductId
                });
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
}