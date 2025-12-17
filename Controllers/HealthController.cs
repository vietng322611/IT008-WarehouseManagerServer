using Microsoft.AspNetCore.Mvc;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController: ControllerBase
{
    [HttpGet]
    [HttpHead]
    public IActionResult Health()
    {
        return Ok();
    }
}