using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

/* Route: api/User
 * Endpoints:
 *      - api/User: GET, POST
 *      - api/User/json: GET
 *      - api/User/[UserId]: GET, PUT, DELETE
 *      - api/User/[UserId]/warehouses: GET
 */

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var content = await service.GetAllAsync();
        return Ok(content);
    }

    [HttpGet("json")]
    public IActionResult GetSampleJson()
    {
        var model = new User
        {
            UserId = 0,
            Username = "User",
            Email = "User@gmail.com",
            JoinDate = DateTime.Now
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
    [HttpGet("{id:int:min(1)}/warehouses")]
    public async Task<IActionResult> GetUserWarehouses([FromRoute] int id)
    {
        try
        {
            var content = await service.GetUserWarehousesAsync(id);
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User content)
    {
        try
        {
            content.UserId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(nameof(GetById), new { id = newContent.UserId }, newContent);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] User updatedContent)
    {
        try
        {
            if (id != updatedContent.UserId)
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