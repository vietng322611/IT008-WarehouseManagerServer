using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Data;
using WarehouseManagerServer.Models.Enums;
using WarehouseManagerServer.Services.Interfaces;

namespace WarehouseManagerServer.Controllers;

/* Route: api/Auth
 * Endpoints:
 * - GET api/Auth/register/json
 * - GET api/Auth/login/json
 * - GET api/Auth/refresh/json
 * - GET api/Auth/logout/json
 * - POST api/Auth/register
 * - POST api/Auth/login
 * - POST api/Auth/refresh
 * - POST api/Auth/logout
 */

public class AuthController(IAuthService service) : ControllerBase
{
    [HttpGet("register/json")]
    public IActionResult GetRegisterJson()
    {
        return Ok(new RegisterDto()
        {
            Username = "Username",
            Email = "Email@gmail.com",
            Password = "Password"
        });
    }

    [HttpGet("login/json")]
    public IActionResult GetLoginJson()
    {
        return Ok(new LoginDto
        {
            Username = "Username",
            Password = "Password"
        });
    }

    [HttpGet("refresh/json")]
    [HttpGet("logout/json")]
    public IActionResult GetRefreshJson()
    {
        return Ok(new RefreshDto
        {
            RefreshToken = "RefreshToken"
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var result = await service.RegisterUser(dto, dto.Password);
            if (result == RegisterEnum.UserAlreadyExists)
                return BadRequest(new { Message = "User already exists" });

            return Ok("Registered successfully");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var user = await service.ValidateUser(dto);
            if (user == null)
                return Unauthorized(new { message = "Invalid Username or Password" });

            var refreshToken = await service.GenerateRefreshToken(user);
            var accessToken = service.GenerateAccessToken(user);

            return Ok(new { accessToken, refreshToken });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshDto dto)
    {
        try
        {
            var user = await service.ValidateRefreshToken(dto);
            if (user == null)
                return Unauthorized(new { Message = "Invalid Refresh Token" });

            var oldToken = user.RefreshTokens.Single(x => x.Token == dto.RefreshToken);
            if (!oldToken.IsActive)
                return Unauthorized(new { Message = "Refresh Token Expired" });

            var newRefreshToken = await service.GenerateRefreshToken(user);
            var newAccessToken = service.GenerateAccessToken(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshDto dto)
    {
        try
        {
            var user = await service.ValidateRefreshToken(dto);
            if (user == null)
                return Unauthorized(new { Message = "Invalid Refresh Token" });

            var oldToken = user.RefreshTokens.Single(x => x.Token == dto.RefreshToken);
            if (!oldToken.IsActive)
                return Unauthorized(new { Message = "Refresh Token Expired" });

            await service.InvalidateRefreshToken(oldToken);
            return Unauthorized();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}