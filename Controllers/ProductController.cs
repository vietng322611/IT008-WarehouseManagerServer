using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int:min(1)}/products")]
public class ProductController(IProductService service) : ControllerBase
{
    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet]
    public async Task<IActionResult> GetWarehouseProducts([FromRoute] int id)
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
    public async Task<IActionResult> Post([FromBody] Product content)
    {
        try
        {
            content.ProductId = 0; // Ignore id in input
            await service.AddAsync(content);

            var newContent = await service.GetByKeyAsync(content.ProductId);
            if (newContent == null) return StatusCode(500, "Error adding new content");

            return CreatedAtAction(
                nameof(GetById),
                new { id = newContent.ProductId },
                Serialize(newContent));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Product updatedContent)
    {
        try
        {
            if (id != updatedContent.ProductId)
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

    private object Serialize(Product content)
    {
        return new
        {
            product_id = content.ProductId,
            name = content.Name,
            category = content.Category == null ? "" : content.Category.Name,
            supplier = content.Supplier == null ? "" : content.Supplier.Name,
            unit_price = content.UnitPrice,
            quantity = content.Quantity,
            expiry_date = content.ExpiryDate,
            warehouse_id = content.WarehouseId,
            category_id = content.CategoryId,
            supplier_id = content.SupplierId
        };
    }
}