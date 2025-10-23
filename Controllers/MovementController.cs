using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Types.Enums;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

/* Route: api/Movement
 * Endpoints:
 *      - api/Movement: POST
 *      - api/Movement/json: GET
 *      - api/Movement/[MovementId]: GET, PUT, DELETE
 */

[ApiController]
[Route("api/[controller]")]
public class MovementController(IMovementService service) : ControllerBase
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
        var model = new Movement
        {
            MovementId = 0,
            ProductId = 0,
            Quantity = 1,
            MovementTypeEnum = MovementTypeEnum.In,
            Date = DateTime.Now
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Movement content)
    {
        try
        {
            content.MovementId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetById), new { id = newContent.MovementId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
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