// Api/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using OrderService.Core.DTOs;
using OrderService.Core.Services;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var result = _authService.Login(request.Email, request.Password);
        if (result is null) return Unauthorized(new { message = "Credenziali non valide" });
        return Ok(result);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] string refreshToken)
    {
        var result = _authService.Refresh(refreshToken);
        if (result is null) return Unauthorized(new { message = "Refresh token non valido o scaduto" });
        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout([FromBody] string refreshToken)
    {
        var revoked = _authService.Logout(refreshToken);
        if (!revoked) return BadRequest(new { message = "Refresh token non trovato" });
        return NoContent();
    }
}
