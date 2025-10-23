using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

/* Route: api/Warehouse
 * Endpoints:
 *      - api/Warehouse: POST
 *      - api/Warehouse/json: GET
 *      - api/Warehouse/[WarehouseId]: GET, PUT, DELETE
 *      - api/Warehouse/[WarehouseId]/users: GET
 */

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(IWarehouseService service) : Controller
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
        var model = new Warehouse
        {
            WarehouseId = 0,
            Name = "Warehouse"
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

    [HttpGet("{id:int:min(1)}/users")]
    public async Task<IActionResult> GetWarehouseUsers([FromRoute] int id)
    {
        try
        {
            var content = await service.GetWarehouseUsersAsync(id);
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Warehouse content)
    {
        try
        {
            content.WarehouseId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetById), new { id = newContent.WarehouseId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Warehouse updatedContent)
    {
        try
        {
            if (id != updatedContent.WarehouseId)
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