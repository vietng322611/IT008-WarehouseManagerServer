using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int:min(1)}/histories")]
public class HistoryController(IHistoryService service) : ControllerBase
{
    // This route can only read for now
    // Endpoint user's cannot directly modify the data in history table
    // As this is used to log user's actions
    
    [WarehousePermission(PermissionEnum.Read)]
    [HttpGet]
    public async Task<IActionResult> GetWarehouseHistories([FromRoute] int warehouseId)
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

    private static object Serialize(History content)
    {
        return new
        {
            history_id = content.HistoryId,
            product_id = content.ProductId,
            user_id = content.UserId,
            product_name = content.Product.Name,
            user_full_name = content.User.FullName,
            quantity = content.Quantity,
            action_type = content.ActionType,
            date = content.Date
        };
    }
}