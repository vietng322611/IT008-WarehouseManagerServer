using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Models.DTOs;
using WarehouseManagerServer.Services.Interfaces;
using WarehouseManagerServer.Types.Enums;

namespace WarehouseManagerServer.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthService service,
    IUserService userService
    ) : ControllerBase
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
            return result switch
            {
                RegisterEnum.UserAlreadyExists => BadRequest(new { message = "User already exists" }),
                RegisterEnum.EmailAlreadyExists => BadRequest(new { message = "Email already exists" }),
                RegisterEnum.InvalidEmail =>  BadRequest(new { message = "Invalid Email" }),
                _ => Ok(new { message = "Registered successfully" })
            };
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
            var (accessToken, expires) = service.GenerateAccessToken(user);

            return Ok(new
            {
                user_id = user.UserId,
                access_token = accessToken,
                refresh_token = refreshToken,
                exprire_in = expires
            });
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
                return Unauthorized(new { message = "Invalid Refresh Token" });

            var oldToken = user.RefreshTokens.Single(x => x.Token == dto.RefreshToken);
            if (!oldToken.IsActive)
                return Unauthorized(new { message = "Refresh Token Expired" });

            var newRefreshToken = await service.GenerateRefreshToken(user);
            var (newAccessToken, expires) = service.GenerateAccessToken(user);

            return Ok(new
            {
                access_token = newAccessToken,
                refresh_token = newRefreshToken,
                expire_in = expires
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
                return Unauthorized(new { message = "Invalid Refresh Token" });

            var oldToken = user.RefreshTokens.Single(x => x.Token == dto.RefreshToken);
            if (!oldToken.IsActive)
                return Unauthorized(new { message = "Refresh Token Expired" });

            await service.InvalidateRefreshToken(oldToken);
            return Unauthorized();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("recovery")]
    public async Task<IActionResult> Recovery([FromBody] string email)
    {
        try
        {
            var user = await userService.GetByUniqueAsync(u => u.Email == email);
            if (user == null)
                return BadRequest(new { message = "Email not associated with any account" });
            
            await service.SendRecoveryCode(user);
            return Ok("Sent recovery code to email: " + email);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}