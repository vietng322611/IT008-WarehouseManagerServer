using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/warehouses/{warehouseId:int:min(1)}/sync")]
public class WarehouseSyncController(IWarehouseService service) : Controller
{
    [WarehousePermission((PermissionEnum.Write))]
    [HttpPost]
    public async Task<IActionResult> Sync([FromRoute] int warehouseId, [FromBody] WarehouseSyncDto syncDto)
    {
        var (success, err) = await service.Sync(warehouseId, syncDto);
        return success ? Ok("Warehouse data synced successfully.") : StatusCode(500, $"Sync failed: {err}");
    }
}