using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.DTOs.Requests;
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
            return Ok(content);
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
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.Authenticated)]
    [HttpGet("warehouses")]
    public async Task<IActionResult> GetUserWarehouses()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var content = await service.GetUserWarehousesAsync(userId);
            return Ok(content);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.Authenticated)]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UserDto updatedContent)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var newUser = await service.UpdateAsync(userId, updatedContent.FullName);
            if (newUser == null)
                return NotFound();
            
            return Ok(newUser);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.Authenticated)]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
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