// Core/DTOs/AuthModels.cs
namespace OrderService.Core.DTOs;

public record LoginRequest(string Email, string Password);

public record TokenResponse(string AccessToken, string RefreshToken, int ExpiresIn);
