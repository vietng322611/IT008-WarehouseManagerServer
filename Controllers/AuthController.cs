using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WarehouseManagerServer.Attributes;
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
    [HttpPost("request-verification")]
    public async Task<IActionResult> RequestVerification([FromBody] RequestCodeDto dto)
    {
        try
        {
            var user = await userService.GetByUniqueAsync(u => u.Email == dto.Email);
            if (user == null)
                return BadRequest(new { message = "Email not associated with any account" });

            await service.SendVerificationCode(user, dto.Type);
            return Ok("Sent recovery code to email: " + dto.Email);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var user = await service.VerifyRecoveryCode(dto.Code);
            if (user == null)
                return BadRequest(new { message = "Invalid verification code" });
            
            var result = await service.RegisterUser(dto.FullName, dto.Email, dto.Password);
            return result switch
            {
                RegisterEnum.EmailAlreadyExists => BadRequest(new { message = "Email already exists" }),
                RegisterEnum.InvalidEmail => BadRequest(new { message = "Invalid Email" }),
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

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        try
        {
            var user = await service.VerifyRecoveryCode(dto.Code);
            if (user == null)
                return BadRequest(new { message = "Invalid verification code" });

            await service.ChangePassword(user, dto.NewPassword);
            return Ok(new { message = "Password successfully reset" });
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [UserPermission(UserPermissionEnum.SameUser)]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            var user = await service.VerifyRecoveryCode(dto.Code);
            if (user == null)
                return BadRequest(new { message = "Invalid verification code" });

            // ISTG this looks so dumb but reuse code should be good right?
            user = await service.ValidateUser(new LoginDto
            {
                Email = user.FullName,
                Password = dto.OldPassword
            });
            if (user == null)
                return BadRequest(new { message = "Old password is incorrect" });

            await service.ChangePassword(user, dto.NewPassword);
            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}