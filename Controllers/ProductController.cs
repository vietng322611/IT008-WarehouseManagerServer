using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.DTOs.Requests;
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
    public async Task<IActionResult> GetWarehouseProducts([FromRoute] int warehouseId)
    {
        var result = await service.GetByWarehouseAsync(warehouseId);
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
    public async Task<IActionResult> Post([FromRoute] int warehouseId, [FromBody] ProductDto content)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            content.ProductId = 0; // Ignore id in input
            content.WarehouseId = warehouseId; // just for safe
            
            var newContent = await service.AddAsync(content, userId);
            return CreatedAtAction(
                nameof(GetById), new { warehouseId, id = newContent.ProductId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("import/{id:int:min(1)}")]
    public async Task<IActionResult> Import([FromRoute] int id, [FromBody] ProductDto updatedContent)
    {
        try
        {
            if (id != updatedContent.ProductId)
                return BadRequest();

            var existingContent = await service.GetByKeyAsync(id);
            if (existingContent == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await service.UpdateAsync(updatedContent, userId, ActionTypeEnum.In);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("export/{id:int:min(1)}")]
    public async Task<IActionResult> Export([FromRoute] int id, [FromBody] ProductDto updatedContent)
    {
        try
        {
            if (id != updatedContent.ProductId)
                return BadRequest();

            var existingContent = await service.GetByKeyAsync(id);
            if (existingContent == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await service.UpdateAsync(updatedContent, userId, ActionTypeEnum.Out);
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
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await service.DeleteAsync(id, userId);
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