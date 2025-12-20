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
                nameof(GetById), new { warehouseId, id = newContent.ProductId }, Serialize(newContent));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut]
    public async Task<IActionResult> UpdateMeta([FromRoute] int id, [FromBody] List<ProductDto> products)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var newProducts = await service.UpdateMetaAsync(products, userId);
            return Ok(newProducts.Select(Serialize));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("import")]
    public async Task<IActionResult> Import([FromBody] List<ProductDto> products)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var newProducts = await service.UpdateQuantityAsync(products, userId, ActionTypeEnum.In);
            return Ok(newProducts.Select(Serialize));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [WarehousePermission(PermissionEnum.Write)]
    [HttpPut("export")]
    public async Task<IActionResult> Export([FromBody] List<ProductDto> products)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var newProducts = await service.UpdateQuantityAsync(products, userId, ActionTypeEnum.Out);
            return Ok(newProducts.Select(Serialize));
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

    private static object Serialize(Product content)
    {
        return new
        {
            product_id = content.ProductId,
            name = content.Name,
            supplier = content.Supplier.Name,
            unit_price = content.UnitPrice,
            quantity = content.Quantity,
            expiry_date = content.ExpiryDate,
            warehouse_id = content.WarehouseId,
            supplier_id = content.SupplierId
        };
    }
}