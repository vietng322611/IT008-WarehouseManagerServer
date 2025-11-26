using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int:min(1)}/suppliers")]
public class SupplierController(ISupplierService service) : ControllerBase
{
    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet]
    public async Task<IActionResult> GetWarehouseSuppliers([FromRoute] int id)
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
    public async Task<IActionResult> Post([FromBody] Supplier content)
    {
        try
        {
            content.SupplierId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetById), new { id = newContent.SupplierId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Supplier updatedContent)
    {
        try
        {
            if (id != updatedContent.SupplierId)
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

    private object Serialize(Supplier content)
    {
        return new
        {
            supplier_id = content.SupplierId,
            warehouse_id = content.WarehouseId,
            name = content.Name,
            contact_info = content.ContactInfo,
            total_imports = content.ProductCount
        };
    }
}