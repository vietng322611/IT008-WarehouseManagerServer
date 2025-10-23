using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

/* Route: api/Category
 * Endpoints:
 * - GET, POST api/Category
 * - GET api/Category/json
 * - GET, PUT, DELETE api/Category/[CategoryId]
 */

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var content = await service.GetAllAsync();
        return Ok(content);
    }

    [HttpGet("json")]
    public IActionResult GetSampleJson()
    {
        var model = new Category
        {
            CategoryId = 0,
            Name = "Category",
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

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Category content)
    {
        try
        {
            content.CategoryId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetById), new { id = newContent.CategoryId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Category updatedContent)
    {
        try
        {
            if (id != updatedContent.CategoryId)
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