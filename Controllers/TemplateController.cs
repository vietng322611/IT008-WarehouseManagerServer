using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Models.Entities;
using WarehouseManagerServer.Models.Enums;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/json")]
public class TemplateController : ControllerBase
{
    [HttpGet("register")]
    public IActionResult GetRegisterJson()
    {
        return Ok(new RegisterDto
        {
            FullName = "John Smith",
            Email = "Email@gmail.com",
            Password = "Password"
        });
    }

    [HttpGet("login")]
    public IActionResult GetLoginJson()
    {
        return Ok(new LoginDto
        {
            Email = "Email@gmail.com",
            Password = "Password"
        });
    }

    [HttpGet("refresh")]
    [HttpGet("logout")]
    public IActionResult GetRefreshJson()
    {
        return Ok(new RefreshDto
        {
            RefreshToken = "RefreshToken"
        });
    }

    [HttpGet("user")]
    public IActionResult GetUserJson()
    {
        return Ok(new User
        {
            UserId = 0,
            FullName = "User",
            Email = "User@gmail.com",
            JoinDate = DateTime.Now
        });
    }

    [HttpGet("warehouse")]
    public IActionResult GetWarehouseJson()
    {
        var model = new Warehouse
        {
            WarehouseId = 0,
            Name = "Warehouse"
        };
        return Ok(model);
    }

    [HttpGet("permission")]
    public IActionResult GetPermissionJson()
    {
        var model = new Permission
        {
            UserId = 0,
            WarehouseId = 0,
            UserPermissions = [PermissionEnum.Read, PermissionEnum.Write, PermissionEnum.Delete, PermissionEnum.Owner]
        };
        return Ok(model);
    }

    [HttpGet("product")]
    public IActionResult GetProductJson()
    {
        var model = new
        {
            ProductId = 1,
            Name = "Product",
            Category = "Category",
            Supplier = "Supplier",
            Quantity = 1,
            ExpiryDate = DateTime.Now,
            WarehouseId = 1,
            CategoryId = 1,
            SupplierId = 1
        };
        return Ok(model);
    }

    [HttpGet("supplier")]
    public IActionResult GetSupplierJson()
    {
        var model = new
        {
            supplier_id = 0,
            warehouse_id = 0,
            name = "Supplier",
            contact_info = "Ho Chi Minh City, Vietnam",
            total_imports = 0
        };
        return Ok(model);
    }

    [HttpGet("movement")]
    public IActionResult GetMovementJson()
    {
        var model = new
        {
            movement_id = 0,
            product = "Product",
            quantity = 1,
            movement_type = MovementTypeEnum.In,
            date = DateTime.Now,
            productId = 0
        };
        return Ok(model);
    }

    [HttpGet("category")]
    public IActionResult GetCategoryJson()
    {
        var model = new Category
        {
            CategoryId = 0,
            WarehouseId = 0,
            Name = "Category"
        };
        return Ok(model);
    }

    [HttpGet("statistic")]
    public IActionResult GetStatisticJson()
    {
        return Ok();
    }
}