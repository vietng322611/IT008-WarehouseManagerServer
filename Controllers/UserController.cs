using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("json")]
    public IActionResult GetSampleJson()
    {
        return Ok(new UserDto
        {
            UserId = 0,
            Username = "User",
            Email = "User@gmail.com",
            JoinDate = DateTime.Now
        });
    }

    [UserPermission]
    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        try
        {
            var content = await service.GetByKeyAsync(id);
            if (content == null) return NotFound();
            return Ok(new UserDto
            {
                UserId = content.UserId,
                Username = content.Username,
                Email = content.Email,
                JoinDate = content.JoinDate
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission]
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

    [UserPermission]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User content)
    {
        try
        {
            content.UserId = 0; // Ignore id in input

            var newContent = await service.AddAsync(content);
            return CreatedAtAction(
                nameof(GetById),
                new { id = newContent.UserId },
                new UserDto
                {
                    UserId = content.UserId,
                    Username = content.Username,
                    Email = content.Email,
                    JoinDate = content.JoinDate
                });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission]
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

    [UserPermission]
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