using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

/* Route: api/Product
 * Endpoints:
 *      - api/Product: POST
 *      - api/Product/json: GET
 *      - api/Product/[ProductId]: GET, PUT, DELETE
 *      - api/Product/warehouse/[WarehouseId]: GET
 */

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService service) : ControllerBase
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
        var model = new Product
        {
            ProductId = 0,
            Name = "Product",
            WarehouseId = 0,
            CategoryId = 0,
            UnitPrice = 1,
            Quantity = 0
        };
        return Ok(model);
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var content = await service.GetByKeyAsync(id);
            if (content == null) return NotFound();
            return Ok(content);
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
            var content = await service.FilterAsync(e => e.WarehouseId == warehouseId);
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Product content)
    {
        try
        {
            content.ProductId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetById), new { id = newContent.ProductId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
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

    [Authorize]
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