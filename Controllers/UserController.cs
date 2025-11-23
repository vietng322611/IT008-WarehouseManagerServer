using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Services.Interfaces;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var content = await service.GetAllAsync();
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.Authenticated)]
    [HttpGet("me")]
    public async Task<IActionResult> GetOwn()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var content = await service.GetByKeyAsync(userId);
            if (content == null) return NotFound();
            return Ok(new User
            {
                UserId = content.UserId,
                FullName = content.FullName,
                Email = content.Email,
                JoinDate = content.JoinDate
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.SameUser)]
    [HttpGet("{userId:int:min(1)}")]
    public async Task<IActionResult> GetById([FromRoute] int userId)
    {
        try
        {
            var content = await service.GetByKeyAsync(userId);
            if (content == null) return NotFound();
            return Ok(new User
            {
                UserId = content.UserId,
                FullName = content.FullName,
                Email = content.Email,
                JoinDate = content.JoinDate
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.SameUser)]
    [HttpGet("{userId:int:min(1)}/warehouses")]
    public async Task<IActionResult> GetUserWarehouses([FromRoute] int userId)
    {
        try
        {
            var content = await service.GetUserWarehousesAsync(userId);
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.SameUser)]
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
                new User
                {
                    UserId = content.UserId,
                    FullName = content.FullName,
                    Email = content.Email,
                    JoinDate = content.JoinDate
                });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.SameUser)]
    [HttpPut("{userId:int:min(1)}")]
    public async Task<IActionResult> Put([FromRoute] int userId, [FromBody] User updatedContent)
    {
        try
        {
            if (userId != updatedContent.UserId)
                return BadRequest();

            var existingContent = await service.GetByKeyAsync(userId);
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

    [UserPermission(UserPermissionEnum.SameUser)]
    [HttpDelete("{userId:int:min(1)}")]
    public async Task<IActionResult> Delete([FromRoute] int userId)
    {
        try
        {
            var success = await service.DeleteAsync(userId);
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